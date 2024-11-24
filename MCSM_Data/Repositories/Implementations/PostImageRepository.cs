using MCSM_Data.Entities;
using MCSM_Data.Repositories.Interfaces;

namespace MCSM_Data.Repositories.Implementations
{
    public class PostImageRepository : Repository<PostImage>, IPostImageRepository
    {
        public PostImageRepository(McsmDbContext context) : base(context)
        {
        }
    }
}
