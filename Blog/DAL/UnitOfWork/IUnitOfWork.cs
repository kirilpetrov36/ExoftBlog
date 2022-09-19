using Blog.DAL.Repositories.Interfaces;
using Blog.DAL.Entities;

namespace Blog.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IArticleRepository ArticleRepository { get; }
        IRepository<Comment> CommentRepository { get; }
        IRepository<CommentLike> CommentLikeRepository { get; }
        IRepository<ArticleLike> ArticleLikeRepository { get; }
        IUserRepository UserRepository { get; }
        IRepository<MediaFile> MediaFileRepository { get; }
        Task SaveChanges(Guid userId);
        Task SaveChanges();
    }

}
