using System.Collections.Generic;
using Business.Dtos;

namespace Business
{
    public interface ICustomerService
    {
        IEnumerable<CustomerDto> GetAll();
        CustomerDto Create(CustomerDto dto);
        CustomerDto Update(CustomerDto dto, out bool changed);
        CustomerDto Delete(CustomerDto dto);
    }
}
