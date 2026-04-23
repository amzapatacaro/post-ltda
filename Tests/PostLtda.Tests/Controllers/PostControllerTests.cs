using System;
using System.Collections.Generic;
using API.Controllers.Post;
using API.Models;
using Business;
using Business.Dtos;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace PostLtda.Tests.Controllers
{
    public class PostControllerTests
    {
        [Fact]
        public void Create_ReturnsBadRequest_WhenCustomerDoesNotExist()
        {
            var mock = new Mock<IPostService>();
            mock.Setup(s => s.Create(It.IsAny<PostDto>()))
                .Throws(new InvalidOperationException("El cliente asociado no existe."));
            var controller = new PostController(mock.Object);

            var result = controller.Create(new PostDto { CustomerId = 999, Body = "x", Type = 1 });

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, bad.StatusCode);
        }

        [Fact]
        public void CreatePosts_ReturnsBadRequest_WhenPostsNull()
        {
            var mock = new Mock<IPostService>();
            var controller = new PostController(mock.Object);

            var result = controller.CreatePosts(new CreatePostsRequest { Posts = null });

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, bad.StatusCode);
        }

        [Fact]
        public void CreatePosts_ReturnsOk_WhenSuccessful()
        {
            var posts = new List<PostDto> { new PostDto { CustomerId = 1, Body = "a", Type = 1 } };
            var returned = new List<PostDto>
            {
                new PostDto { PostId = 5, CustomerId = 1, Body = "a", Type = 1, Category = "Farándula" }
            };
            var mock = new Mock<IPostService>();
            mock.Setup(s => s.CreatePosts(posts)).Returns(returned);
            var controller = new PostController(mock.Object);

            var result = controller.CreatePosts(new CreatePostsRequest { Posts = posts });

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(returned, ok.Value);
        }

        [Fact]
        public void CreatePosts_ReturnsBadRequest_WhenServiceThrowsInvalidOperation()
        {
            var posts = new List<PostDto> { new PostDto { CustomerId = 1 } };
            var mock = new Mock<IPostService>();
            mock.Setup(s => s.CreatePosts(It.IsAny<IEnumerable<PostDto>>()))
                .Throws(new InvalidOperationException("El cliente asociado no existe."));
            var controller = new PostController(mock.Object);

            var result = controller.CreatePosts(new CreatePostsRequest { Posts = posts });

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, bad.StatusCode);
        }
    }
}
