using Blog.DAL.Entities;
using Blog.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Repositories
{
    public class UserFileRepository : Repository<UserFile>, IUserFileRepository
    {
        private readonly AppDbContext _context;
        public UserFileRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<UserFile>> GetUserFilesAsync(Guid userId, CancellationToken token = default)
        {
            return await _context.UserFiles.Where(uf => uf.CreatedBy == userId).ToListAsync();
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
