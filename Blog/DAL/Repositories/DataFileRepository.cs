using Blog.DAL.Entities;
using Blog.DAL.Repositories.Interfaces;

namespace Blog.DAL.Repositories
{
    public class DataFileRepository : Repository<DataFile>, IDataFileRepository 
    {
        private readonly AppDbContext _context;
        public DataFileRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public virtual void DeleteAsync(DataFile file, CancellationToken token = default)
        {
            if (file != null)
            {
                _context.Set<DataFile>().Remove(file);
            }
        }
    }
}
