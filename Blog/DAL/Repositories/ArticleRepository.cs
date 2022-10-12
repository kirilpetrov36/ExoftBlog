using Blog.DAL.Entities;
using Blog.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Repositories
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        private readonly AppDbContext _context;

        public ArticleRepository (AppDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Article> GetAsync(Guid id, CancellationToken token = default)
        {
            try
            {
                return await _context.Articles
                    .Include(p => p.Likes)
                        .ThenInclude(p => p.User)
                    .Include(p => p.Comments)
                        .ThenInclude(p => p.User)
                    .Include(p => p.ArticleFiles)
                    .SingleOrDefaultAsync(x => x.Id == id);
            }
            catch
            {
                return null;
            }
        }

        public override async Task<IEnumerable<Article>> GetListAsync(CancellationToken token = default)
        {
            return await _context.Articles
                    .Include(p => p.Likes)
                    .Include(p => p.Comments)
                    .Include(p => p.ArticleFiles)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetArticlesBySubscriptionAsync(Guid currentUserId, CancellationToken token = default)
        {
            User user = await _context.Users
                            .Include(u => u.UserSubscriptions)
                            .FirstOrDefaultAsync(user => user.Id == currentUserId);

            List<Guid> subscribedUsersId = new List<Guid>();
            foreach(UserSubscription elem in user.UserSubscriptions)
            {
                subscribedUsersId.Add(elem.UserToSubscribeId);
            }
            //return await _context.Articles.Where(article => subscriptions.Any(s => s.UserToSubscribeId == article.CreatedBy))
            return await _context.Articles.Where(article => subscribedUsersId.Contains(article.CreatedBy))
                    .Include(p => p.Likes)
                    .Include(p => p.Comments)
                    .Include(p => p.ArticleFiles)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetMostComentableAsync(CancellationToken token = default)
        {
            return await _context.Articles
                    .Include(p => p.Likes)
                    .Include(p => p.Comments)
                    .Include(p => p.ArticleFiles)
                    .OrderByDescending(p => p.Comments.Count())
                    .ToListAsync();
                    
        }

        public async Task<IEnumerable<Article>> GetMostLikeableAsync(CancellationToken token = default)
        {
            return await _context.Articles
                    .Include(p => p.Likes)
                    .Include(p => p.Comments)
                    .Include(p => p.ArticleFiles)
                    .OrderByDescending(p => p.Likes.Count())
                    .ToListAsync();
        }
    }
}

