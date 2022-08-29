using Blog.DAL.Entities;
using Blog.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository (AppDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Post> GetAsync(long id, CancellationToken token = default)
        {
            if (id == null)
            {
                return null;
            }

            try
            {

                return await _context.Posts
                    .Include(p => p.Likes)
                        .ThenInclude(p => p.User)
                    .Include(p => p.Comments)
                        .ThenInclude(p => p.User)
                    .Include(p => p.Images)
                    .SingleOrDefaultAsync(x => x.Id == id);

            }
            catch
            {
                return null;
            }
        }

        public override async Task<IEnumerable<Post>> GetListAsync(CancellationToken token = default)
        {
            return await _context.Posts
                    .Include(p => p.Likes)
                        .ThenInclude(p => p.User)
                    .Include(p => p.Comments)
                        .ThenInclude(p => p.User)
                    .Include(p => p.Images)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetMostComentableAsync(CancellationToken token = default)
        {
            return await _context.Posts
                    .Include(p => p.Likes)
                        .ThenInclude(p => p.User)
                    .Include(p => p.Comments)
                        .ThenInclude(p => p.User)
                    .Include(p => p.Images)
                    .OrderByDescending(p => p.Comments.Count())
                    .ToListAsync();
                    
        }

        public async Task<IEnumerable<Post>> GetMostLikeableAsync(CancellationToken token = default)
        {
            return await _context.Posts
                    .Include(p => p.Likes)
                        .ThenInclude(p => p.User)
                    .Include(p => p.Comments)
                        .ThenInclude(p => p.User)
                    .Include(p => p.Images)
                    .OrderByDescending(p => p.Likes.Count())
                    .ToListAsync();
        }
    }
}

