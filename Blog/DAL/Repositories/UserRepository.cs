using Blog.DAL.Entities;
using Blog.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Repositories
{
    public class UserRepository : Repository<User>, IRepository<User>
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) : base(context){
            _context = context;
        }

        public override async Task<User> GetAsync(long id, CancellationToken token = default)
        {
            try
            {
                return await _context.Users
                    .Include(p => p.Comments)
                        .ThenInclude(p => p.Post)
                    .Include(p => p.PostLikes)
                    .Include(p => p.CommentLikes)
                    .SingleOrDefaultAsync(x => x.Id == id);

            }
            catch
            {
                return null;
            }
        }

        public override async Task<IEnumerable<User>> GetListAsync(CancellationToken token = default)
        {
            return await _context.Users
                    .Include(p => p.Comments)
                    .Include(p => p.PostLikes)
                    .Include(p => p.CommentLikes)
                    .ToListAsync();
        }
    }
}
