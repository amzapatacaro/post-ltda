using System;
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
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var customerExists = _context
                .Customer.AsNoTracking()
                .Any(c => c.CustomerId == entity.CustomerId);

            if (!customerExists)
                throw new InvalidOperationException("El cliente asociado no existe.");

            entity.Body = PostHelper.NormalizeBody(entity.Body);
            entity.Category = PostHelper.ResolveCategory(entity.Type, entity.Category);

            return base.Create(entity);
        }
    }
}
