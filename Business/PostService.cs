using DataAccess;
using DataAccess.Data;

namespace Business
{
    public class PostService : BaseService<Post>, IPostService
    {
        public PostService(IBaseModel<Post> baseModel)
            : base(baseModel) { }
    }
}
