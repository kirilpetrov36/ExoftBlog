using Blog.DAL.Repositories;
using Blog.DAL.Repositories.Interfaces;
using Blog.DAL.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public IArticleRepository ArticleRepository => new ArticleRepository(_context);
        public IRepository<Comment> CommentRepository => new CommentRepository(_context);
        public IRepository<CommentLike> CommentLikeRepository => new CommentLikeRepository(_context);
        public IRepository<ArticleLike> ArticleLikeRepository => new ArticleLikeRepository(_context);
        public IUserRepository UserRepository => new UserRepository(_context);
        public IRepository<MediaFile> MediaFileRepository => new Repository<MediaFile>(_context);

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

        public async Task SaveChanges(Guid userId)
        {
            IEnumerable<EntityEntry> entries = _context.ChangeTracker.Entries();
            if (!entries.Any())
                await _context.SaveChangesAsync();

            foreach (EntityEntry entity in entries)
            {
                BaseEntity baseEntity = (BaseEntity)entity.Entity;

                if (entity.State == EntityState.Added)
                {
                    baseEntity.CreatedAt = DateTime.Now;
                    baseEntity.CreatedBy = userId;
                }
                else if (entity.State == EntityState.Modified)
                {
                    baseEntity.UpdatedAt = DateTime.Now;
                    baseEntity.UpdatedBy = userId;
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
