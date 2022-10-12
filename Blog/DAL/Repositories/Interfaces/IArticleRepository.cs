using Blog.DAL.Entities;

namespace Blog.DAL.Repositories.Interfaces
{
    public interface IArticleRepository : IRepository<Article>
    {
        Task<IEnumerable<Article>> GetMostComentableAsync(CancellationToken token = default);
        Task<IEnumerable<Article>> GetMostLikeableAsync(CancellationToken token = default);
        Task<IEnumerable<Article>> GetArticlesBySubscriptionAsync(Guid currentUserId, CancellationToken token = default);
    }
}
