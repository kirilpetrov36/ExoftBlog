using Blog.DAL.Entities;

namespace Blog.DAL.Repositories.Interfaces
{
    public interface IArticleRepository : IRepository<Article>
    {
        Task<IEnumerable<Article>> GetMostComentableAsync(CancellationToken token = default);
        Task<IEnumerable<Article>> GetMostLikeableAsync(CancellationToken token = default);
        Task<IEnumerable<Article>> GetArticlesBySubscriptionAsync(Guid currentUserId, CancellationToken token = default);
        Task<IEnumerable<Article>> GetUserArticlesAsync(Guid userId, CancellationToken token = default);
        Task<IEnumerable<Article>> SearchArticles(string searchInput, CancellationToken token = default);
        Task<int> GetLikesAmount(Guid id, CancellationToken token = default);
        Task<int> GetCommentsAmount(Guid id, CancellationToken token = default);
    }
}
