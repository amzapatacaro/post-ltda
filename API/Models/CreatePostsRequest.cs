using System.Collections.Generic;
using Business.Dtos;

namespace API.Models
{
    public class CreatePostsRequest
    {
        public List<PostDto> Posts { get; set; }
    }
}
