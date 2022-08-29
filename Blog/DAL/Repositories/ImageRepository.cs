using Blog.DAL.Entities;
using Blog.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Repositories
{
    public class ImageRepository : Repository<Image>, IRepository<Image>
    {
        public ImageRepository(AppDbContext context) : base(context) { }
    }
}
