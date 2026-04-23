using System.Collections.Generic;
using Business;
using Microsoft.AspNetCore.Mvc;
using PostEntity = DataAccess.Data.Post;

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
        public IEnumerable<PostEntity> GetAll()
        {
            return _postService.GetAll();
        }

        [HttpPost]
        public PostEntity Create([FromBody] PostEntity entity)
        {
            return _postService.Create(entity);
        }

        [HttpPut]
        public PostEntity Update([FromBody] PostEntity entity)
        {
            return _postService.Update(entity.PostId, entity, out bool changed);
        }

        [HttpDelete]
        public PostEntity Delete([FromBody] PostEntity entity)
        {
            return _postService.Delete(entity);
        }
    }
}
