using Blog.DAL.Entities;
using Blog.DAL.Repositories.Interfaces;

namespace Blog.DAL.Repositories
{
    public class UserFileRepository : Repository<UserFile>, IUserFileRepository
    {
        private readonly AppDbContext _context;
        public UserFileRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public virtual void DeleteAsync(UserFile file, CancellationToken token = default)
        {
            if (file != null)
            {
                _context.Set<UserFile>().Remove(file);
            }
        }
    }
}
