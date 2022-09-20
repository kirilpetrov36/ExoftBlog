using Blog.DAL.Entities;

namespace Blog.DAL.Repositories.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<int?> GetArticleCommentsAmount(Guid ArticleId);
    }
}
