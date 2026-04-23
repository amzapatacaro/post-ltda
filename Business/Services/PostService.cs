using System;
using System.Collections.Generic;
using System.Linq;
using Business.Dtos;
using Business.Helpers;
using Business.Mapping;
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

        IEnumerable<PostDto> IPostService.GetAll()
        {
            return base.GetAll().Select(p => p.ToDto());
        }

        public override Post Create(Post entity)
        {
            return CreateNewPost(entity);
        }

        public PostDto Create(PostDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            return Create(dto.ToPost()).ToDto();
        }

        public IReadOnlyList<PostDto> CreatePosts(IEnumerable<PostDto> posts)
        {
            if (posts == null)
                throw new ArgumentNullException(nameof(posts));

            var list = posts as IList<PostDto> ?? posts.ToList();
            using (var trx = _context.Database.BeginTransaction())
            {
                try
                {
                    var results = new List<PostDto>();
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

        public PostDto Update(PostDto dto, out bool changed)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            return Update(dto.PostId, dto.ToPost(), out changed).ToDto();
        }

        public PostDto Delete(PostDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            return Delete(dto.ToPost()).ToDto();
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
