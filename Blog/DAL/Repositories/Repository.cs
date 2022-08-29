using Blog.DAL.Repositories.Interfaces;
using Blog.DAL.Entities;
using Microsoft.EntityFrameworkCore;


namespace Blog.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public virtual async Task<IEnumerable<T>> GetListAsync(CancellationToken token = default)
        {
            if (_context.Posts == null)
            {
                return null;
            }

            return await _context.Set<T>().ToListAsync();

        }

        public virtual async Task<T> GetAsync(long id, CancellationToken token = default)
        {
            if (id == null)
            {
                return null;
            }

            return await _context.Set<T>().FindAsync(id);

        }

        public virtual async Task CreateAsync(T item, CancellationToken token = default)
        {
            _context.Set<T>().Add(item);
        }

        public virtual async Task<T> UpdateAsync(T item, CancellationToken token = default)
        {
            _context.Set<T>().Update(item);
            return item;
        }

        public virtual async Task DeleteAsync(long id, CancellationToken token = default)
        {
            T entity = await _context.Set<T>().FindAsync(id);

            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
            }
        }
    }
}
