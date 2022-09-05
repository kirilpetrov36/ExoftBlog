using Blog.DAL.Entities;
using Blog.DAL.Repositories.Interfaces;

namespace Blog.DAL.Repositories
{
    public class MediaFileRepository : Repository<MediaFile>, IRepository<MediaFile>
    {
        public MediaFileRepository(AppDbContext context) : base(context) { }
    }
}
