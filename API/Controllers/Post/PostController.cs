using System;
using API.Models;
using Business;
using Business.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Post
{
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_postService.GetAll());
        }

        [HttpPost]
        public IActionResult Create([FromBody] PostDto dto)
        {
            try
            {
                return Ok(_postService.Create(dto));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("List")]
        public IActionResult CreatePosts([FromBody] CreatePostsRequest request)
        {
            try
            {
                if (request?.Posts == null)
                    return BadRequest(new { message = "Posts es obligatorio." });
                return Ok(_postService.CreatePosts(request.Posts));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] PostDto dto)
        {
            return Ok(_postService.Update(dto, out bool changed));
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] PostDto dto)
        {
            return Ok(_postService.Delete(dto));
        }
    }
}
