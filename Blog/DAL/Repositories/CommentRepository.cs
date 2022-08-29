using Blog.DAL.Entities;
using Blog.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Repositories
{
    public class CommentRepository: Repository<Comment>, IRepository<Comment>
    {
        private readonly AppDbContext _context;
        public CommentRepository(AppDbContext context) : base(context) { 
            _context = context;
        }
        
        public override async Task<Comment> GetAsync(long id, CancellationToken token = default)
        {
            try
            {

                return await _context.Comments
                    .Include(p => p.User)
                    .Include(p => p.Post)
                    .Include(p => p.ParentComment)
                    .Include(p => p.ChildComments)
                    .Include(p => p.Likes)
                    .SingleOrDefaultAsync(x => x.Id == id);

            }
            catch
            {
                return null;
            }
        }

        public override async Task<IEnumerable<Comment>> GetListAsync(CancellationToken token = default)
        {
            return await _context.Comments
                    .Include(p => p.User)
                    .Include(p => p.Post)
                    .Include(p => p.ParentComment)
                    .Include(p => p.ChildComments)
                    .Include(p => p.Likes)
                    .ToListAsync();
        }

    }
}
