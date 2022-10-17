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
            return await _context.Articles
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .Include(p => p.ArticleFiles)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Article>> SearchArticles(string searchInput, CancellationToken token = default)
        {
            return await _context.Articles
                .Where(article => (article.Title.Contains(searchInput) || article.Content.Contains(searchInput)))
                .Include(p => p.User)
                    .ThenInclude(p => p.Files)
                .Include(p => p.ArticleFiles)
                .ToListAsync();
        }

        public override async Task<IEnumerable<Article>> GetListAsync(CancellationToken token = default)
        {
            return await _context.Articles
                    .Include(p => p.User)
                        .ThenInclude(p => p.Files)
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
                    .Include(p => p.User)
                    .Include(p => p.ArticleFiles)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetMostComentableAsync(CancellationToken token = default)
        {
            return await _context.Articles
                    .Include(p => p.User)
                    .Include(p => p.ArticleFiles)
                    .OrderByDescending(p => p.Comments.Count())
                    .ToListAsync();                   
        }

        public async Task<IEnumerable<Article>> GetMostLikeableAsync(CancellationToken token = default)
        {
            return await _context.Articles
                    .Include(p => p.User)
                    .Include(p => p.ArticleFiles)
                    .OrderByDescending(p => p.Likes.Count())
                    .ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetUserArticlesAsync(Guid userId, CancellationToken token = default)
        {
            return await _context.Articles
                .Where(a => a.CreatedBy == userId)
                .Include(p => p.User)
                    .ThenInclude(p => p.Files)
                .Include(p => p.ArticleFiles)
                .ToListAsync();
        }

        public async Task<int> GetLikesAmount(Guid id, CancellationToken token = default)
        {
            Article article = await _context.Articles
                .Include(p => p.Likes)
                .FirstOrDefaultAsync(p => p.Id == id);
            
            return article.Likes.Count();
        }

        public async Task<int> GetCommentsAmount(Guid id, CancellationToken token = default)
        {
            Article article = await _context.Articles
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);

            return article.Comments.Count();
        }
    }
}

