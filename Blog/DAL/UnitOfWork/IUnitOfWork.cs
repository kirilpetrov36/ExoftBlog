using Blog.DAL.Repositories.Interfaces;
using Blog.DAL.Entities;

namespace Blog.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IPostRepository PostRepository { get; }
        IRepository<Comment> CommentRepository { get; }
        IRepository<CommentLike> CommentLikeRepository { get; }
        IRepository<PostLike> PostLikeRepository { get; }
        IUserRepository UserRepository { get; }
        IRepository<MediaFile> MediaFileRepository { get; }
        Task SaveChanges();
    }

}
