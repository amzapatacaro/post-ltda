using System;
using Business;
using Business.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Customer
{
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_customerService.GetAll());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CustomerDto dto)
        {
            try
            {
                var created = _customerService.Create(dto);
                return Ok(created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (DuplicateCustomerNameException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] CustomerDto dto)
        {
            var updated = _customerService.Update(dto, out bool changed);
            if (updated == null)
                return NotFound(new { message = "Cliente no encontrado." });
            return Ok(updated);
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] CustomerDto dto)
        {
            var deleted = _customerService.Delete(dto);
            if (deleted == null)
                return NotFound(new { message = "Cliente no encontrado." });
            return Ok(deleted);
        }
    }
}
