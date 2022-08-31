using Blog.DAL.Repositories;
using Blog.DAL.Repositories.Interfaces;
using Blog.DAL.Entities;

namespace Blog.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public IPostRepository PostRepository => new PostRepository(_context);
        public IRepository<Comment> CommentRepository => new CommentRepository(_context);
        public IRepository<CommentLike> CommentLikeRepository => new CommentLikeRepository(_context);
        public IRepository<PostLike> PostLikeRepository => new PostLikeRepository(_context);
        public IUserRepository UserRepository => new UserRepository(_context);
        public IRepository<Image> ImageRepository => new Repository<Image>(_context);

        private bool _disposed;
        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

    }
}
