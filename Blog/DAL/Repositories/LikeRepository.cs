using Blog.DAL.Entities;
using Blog.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Repositories
{
    public class ArticleLikeRepository : Repository<ArticleLike>, IRepository<ArticleLike>
    {
        private readonly AppDbContext _context;
        public ArticleLikeRepository(AppDbContext context) : base(context) {
            _context = context;
        }

        public override async Task<ArticleLike> GetAsync(Guid id, CancellationToken token = default)
        {
            try
            {
                return await _context.ArticleLikes
                    .Include(p => p.User)                       
                    .Include(p => p.Article)
                    .SingleOrDefaultAsync(x => x.Id == id);
            }
            catch
            {
                return null;
            }
        }

        public override async Task<IEnumerable<ArticleLike>> GetListAsync(CancellationToken token = default)
        {
            return await _context.ArticleLikes
                    .Include(p => p.User)
                    .Include(p => p.Article)
                    .ToListAsync();
        }
    }

    public class CommentLikeRepository : Repository<CommentLike>, IRepository<CommentLike>
    {
        private readonly AppDbContext _context;
        public CommentLikeRepository(AppDbContext context) : base(context) { 
            _context = context;
        }

        public override async Task<CommentLike> GetAsync(Guid id, CancellationToken token = default)
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
