using Blog.DAL.Repositories.Interfaces;
using Blog.DAL.Entities;

namespace Blog.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IArticleRepository ArticleRepository { get; }
        ICommentRepository CommentRepository { get; }
        ICommentLikeRepository CommentLikeRepository { get; }
        IArticleLikeRepository ArticleLikeRepository { get; }
        IUserRepository UserRepository { get; }
        IArticleFileRepository ArticleFileRepository { get; }
        Task SaveChanges(Guid userId);
        Task SaveChanges();
    }

}
