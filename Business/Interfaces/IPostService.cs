using System.Collections.Generic;
using Business.Dtos;

namespace Business
{
    public interface IPostService
    {
        IEnumerable<PostDto> GetAll();
        PostDto Create(PostDto dto);
        IReadOnlyList<PostDto> CreatePosts(IEnumerable<PostDto> posts);
        PostDto Update(PostDto dto, out bool changed);
        PostDto Delete(PostDto dto);
    }
}
