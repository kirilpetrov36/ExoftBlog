using Blog.DAL.Entities;

namespace Blog.DAL.Repositories.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<IEnumerable<Post>> GetMostComentableAsync(CancellationToken token = default);
        Task<IEnumerable<Post>> GetMostLikeableAsync(CancellationToken token = default);
    }
}
