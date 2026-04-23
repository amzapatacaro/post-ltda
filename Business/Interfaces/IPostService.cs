using System.Collections.Generic;
using DataAccess.Data;

namespace Business
{
    public interface IPostService : IBaseService<Post>
    {
        IReadOnlyList<Post> CreatePosts(IEnumerable<Post> posts);
    }
}
