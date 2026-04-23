using System;
using System.Collections.Generic;
using System.Linq;
using Business.Dtos;
using Business.Mapping;
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

        IEnumerable<CustomerDto> ICustomerService.GetAll()
        {
            return base.GetAll().Select(c => c.ToDto());
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

        public CustomerDto Create(CustomerDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            return Create(dto.ToCustomer()).ToDto();
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

        public CustomerDto Update(CustomerDto dto, out bool changed)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            return Update(dto.CustomerId, dto.ToCustomer(), out changed).ToDto();
        }

        public override Customer Delete(Customer entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            using (var trx = _context.Database.BeginTransaction())
            {
                try
                {
                    var customer = _context.Customer.Find(entity.CustomerId);
                    if (customer == null)
                    {
                        return null;
                    }

                    var posts = _context
                        .Post.Where(p => p.CustomerId == entity.CustomerId)
                        .ToList();

                    if (posts.Count > 0)
                    {
                        _context.Post.RemoveRange(posts);
                    }

                    _context.Customer.Remove(customer);
                    _context.SaveChanges();
                    trx.Commit();
                    return customer;
                }
                catch
                {
                    trx.Rollback();
                    throw;
                }
            }
        }

        public CustomerDto Delete(CustomerDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            return Delete(dto.ToCustomer()).ToDto();
        }
    }
}
