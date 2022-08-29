using Blog.DAL.Entities;
using Blog.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Repositories
{
    public class PostLikeRepository : Repository<PostLike>, IRepository<PostLike>
    {
        private readonly AppDbContext _context;
        public PostLikeRepository(AppDbContext context) : base(context) {
            _context = context;
        }

        public override async Task<PostLike> GetAsync(long id, CancellationToken token = default)
        {
            try
            {

                return await _context.PostLikes
                    .Include(p => p.User)                       
                    .Include(p => p.Post)
                    .SingleOrDefaultAsync(x => x.Id == id);

            }
            catch
            {
                return null;
            }
        }

        public override async Task<IEnumerable<PostLike>> GetListAsync(CancellationToken token = default)
        {
            return await _context.PostLikes
                    .Include(p => p.User)
                    .Include(p => p.Post)
                    .ToListAsync();
        }
    }

    public class CommentLikeRepository : Repository<CommentLike>, IRepository<CommentLike>
    {
        private readonly AppDbContext _context;
        public CommentLikeRepository(AppDbContext context) : base(context) { 
            _context = context;
        }

        public override async Task<CommentLike> GetAsync(long id, CancellationToken token = default)
        {
            try
            {

                return await _context.CommentLikes
                    .Include(p => p.User)
                    .Include(p => p.Comment)
                    .SingleOrDefaultAsync(x => x.Id == id);

            }
            catch
            {
                return null;
            }
        }

        public override async Task<IEnumerable<CommentLike>> GetListAsync(CancellationToken token = default)
        {
            return await _context.CommentLikes
                    .Include(p => p.User)
                    .Include(p => p.Comment)
                    .ToListAsync();
        }

    }
}
