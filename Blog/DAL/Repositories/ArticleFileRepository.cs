using Blog.DAL.Entities;
using Blog.DAL.Repositories.Interfaces;

namespace Blog.DAL.Repositories
{
    public class ArticleFileRepository : Repository<ArticleFile>, IArticleFileRepository 
    {
        private readonly AppDbContext _context;
        public ArticleFileRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public virtual void DeleteAsync(ArticleFile file, CancellationToken token = default)
        {
            if (file != null)
            {
                _context.Set<ArticleFile>().Remove(file);
            }
        }
    }
}
