using System.Collections.Generic;
using System.Linq;
using Business;
using Microsoft.AspNetCore.Mvc;
using CustomerEntity = DataAccess.Data.Customer;

namespace API.Controllers.Customer
{
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly BaseService<CustomerEntity> _customerService;

        public CustomerController(BaseService<CustomerEntity> customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public IEnumerable<CustomerEntity> GetAll()
        {
            return _customerService.GetAll().ToList();
        }

        [HttpPost]
        public CustomerEntity Create([FromBody] CustomerEntity entity)
        {
            return _customerService.Create(entity);
        }

        [HttpPut]
        public CustomerEntity Update([FromBody] CustomerEntity entity)
        {
            return _customerService.Update(entity.CustomerId, entity, out bool changed);
        }

        [HttpDelete]
        public CustomerEntity Delete([FromBody] CustomerEntity entity)
        {
            return _customerService.Delete(entity);
        }
    }
}
