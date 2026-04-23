using System.Collections.Generic;
using DataAccess.Data;

namespace API.Models
{
    public class CreatePostsRequest
    {
        public List<Post> Posts { get; set; }
    }
}
