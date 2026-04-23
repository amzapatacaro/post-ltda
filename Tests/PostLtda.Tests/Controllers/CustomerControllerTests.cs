using System;
using API.Controllers.Customer;
using Business;
using Business.Dtos;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace PostLtda.Tests.Controllers
{
    public class CustomerControllerTests
    {
        [Fact]
        public void Create_ReturnsOk_WhenSuccessful()
        {
            var created = new CustomerDto { CustomerId = 1, Name = "Acme" };
            var mock = new Mock<ICustomerService>();
            mock.Setup(s => s.Create(It.IsAny<CustomerDto>())).Returns(created);
            var controller = new CustomerController(mock.Object);

            var result = controller.Create(new CustomerDto { Name = "Acme" });

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(created, ok.Value);
        }

        [Fact]
        public void Create_ReturnsConflict_WhenDuplicateName()
        {
            var mock = new Mock<ICustomerService>();
            mock.Setup(s => s.Create(It.IsAny<CustomerDto>())).Throws(new DuplicateCustomerNameException());
            var controller = new CustomerController(mock.Object);

            var result = controller.Create(new CustomerDto { Name = "Dup" });

            var conflict = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(409, conflict.StatusCode);
        }

        [Fact]
        public void Create_ReturnsBadRequest_WhenArgumentException()
        {
            var mock = new Mock<ICustomerService>();
            mock.Setup(s => s.Create(It.IsAny<CustomerDto>())).Throws(new ArgumentException("El nombre es obligatorio."));
            var controller = new CustomerController(mock.Object);

            var result = controller.Create(new CustomerDto { Name = "" });

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, bad.StatusCode);
        }

        [Fact]
        public void Update_ReturnsNotFound_WhenCustomerMissing()
        {
            var changed = false;
            var mock = new Mock<ICustomerService>();
            mock.Setup(s => s.Update(It.IsAny<CustomerDto>(), out changed))
                .Returns((CustomerDto)null);
            var controller = new CustomerController(mock.Object);

            var result = controller.Update(new CustomerDto { CustomerId = 99, Name = "Nadie" });

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFound.StatusCode);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenCustomerMissing()
        {
            var mock = new Mock<ICustomerService>();
            mock.Setup(s => s.Delete(It.IsAny<CustomerDto>())).Returns((CustomerDto)null);
            var controller = new CustomerController(mock.Object);

            var result = controller.Delete(new CustomerDto { CustomerId = 42 });

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFound.StatusCode);
        }
    }
}
