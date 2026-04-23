using System;
using System.Collections.Generic;
using System.Linq;
using Business.Helpers;
using DataAccess;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace Business
{
    public class PostService : BaseService<Post>, IPostService
    {
        private readonly JujuTestContext _context;

        public PostService(JujuTestContext context, IBaseModel<Post> baseModel)
            : base(baseModel)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public override Post Create(Post entity)
        {
            return CreateNewPost(entity);
        }

        public IReadOnlyList<Post> CreatePosts(IEnumerable<Post> posts)
        {
            if (posts == null)
                throw new ArgumentNullException(nameof(posts));

            var list = posts as IList<Post> ?? posts.ToList();
            using (var trx = _context.Database.BeginTransaction())
            {
                try
                {
                    var results = new List<Post>();
                    foreach (var item in list)
                    {
                        results.Add(Create(item));
                    }

                    trx.Commit();
                    return results;
                }
                catch
                {
                    trx.Rollback();
                    throw;
                }
            }
        }

        private Post CreateNewPost(Post entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var customerExists = _context
                .Customer.AsNoTracking()
                .Any(c => c.CustomerId == entity.CustomerId);

            if (!customerExists)
                throw new InvalidOperationException("El cliente asociado no existe.");

            entity.Body = PostHelper.NormalizeBody(entity.Body);
            entity.Category = PostHelper.ResolveCategory(entity.Type, entity.Category);

            return _baseModel.Create(entity);
        }
    }
}
