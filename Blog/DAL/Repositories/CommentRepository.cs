using Blog.DAL.Entities;
using Blog.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Repositories
{
    public class CommentRepository: Repository<Comment>, ICommentRepository
    {
        private readonly AppDbContext _context;
        public CommentRepository(AppDbContext context) : base(context) { 
            _context = context;
        }
        
        public override async Task<Comment> GetAsync(Guid id, CancellationToken token = default)
        {
            try
            {
                return await _context.Comments
                    .Include(p => p.User)
                    .Include(p => p.Article)
                    .Include(p => p.ParentComment)
                    .Include(p => p.ChildComments)
                    .Include(p => p.Likes)
                    .SingleOrDefaultAsync(x => x.Id == id);
            }
            catch
            {
                return null;
            }
        }

        public override async Task<IEnumerable<Comment>> GetListAsync(CancellationToken token = default)
        {
            try
            {
                return await _context.Comments
                    .Include(p => p.User)
                    .Include(p => p.Article)
                    .Include(p => p.ParentComment)
                    .Include(p => p.ChildComments)
                    .Include(p => p.Likes)
                    .ToListAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<int?> GetArticleCommentsAmount(Guid ArticleId)
        {
            try
            {
                return await _context.Comments
                    .Where(p => p.Id == ArticleId)
                    .Include(p => p.ChildComments)                    
                    .CountAsync();
            }
            catch
            {
                return null;
            }
        }

    }
}
