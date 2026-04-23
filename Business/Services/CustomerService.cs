using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace Business
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        private readonly JujuTestContext _context;

        public CustomerService(JujuTestContext context, IBaseModel<Customer> baseModel)
            : base(baseModel)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public override IEnumerable<Customer> GetAll()
        {
            return _baseModel.GetAll.ToList();
        }

        public override Customer Create(Customer entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var normalized = (entity.Name ?? string.Empty).Trim();
            if (normalized.Length == 0)
                throw new ArgumentException("El nombre es obligatorio.", nameof(entity));

            var exists = _context
                .Customer.AsNoTracking()
                .Any(c => c.Name != null && c.Name.Trim().ToLower() == normalized.ToLower());

            if (exists)
                throw new DuplicateCustomerNameException();

            entity.Name = normalized;
            return base.Create(entity);
        }

        public override Customer Update(object id, Customer entity, out bool changed)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var original = _baseModel.FindById(id);
            if (original == null)
            {
                changed = false;
                return null;
            }

            var customerId = id is int cid ? cid : Convert.ToInt32(id);
            entity.CustomerId = customerId;
            return _baseModel.Update(entity, original, out changed);
        }
    }
}
