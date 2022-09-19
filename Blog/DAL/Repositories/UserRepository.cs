using Blog.DAL.Entities;
using Blog.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context){
            _context = context;
        }

        public virtual async void DeleteAllUserRefreshTokens(string refreshToken, CancellationToken cancellationToken = default)
        {
            var user = await GetUserByRefreshToken(refreshToken);
            user.RefreshTokens.Clear();
            _context.Update(user);
            _context.SaveChanges();
        }

        public virtual async Task<User> GetUserByRefreshToken(string refreshToken, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .SingleOrDefaultAsync(user => user.RefreshTokens.Any(rtokens => rtokens.Token == refreshToken));
        }

        public async Task<User> GetAsync(Guid id, CancellationToken token = default)
        {
            try
            {
                return await _context.Users
                    .Include(p => p.Comments)
                        .ThenInclude(p => p.Article)
                    .Include(p => p.ArticleLikes)
                    .Include(p => p.CommentLikes)
                    .SingleOrDefaultAsync(x => x.Id == id);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<User>> GetListAsync(CancellationToken token = default)
        {
            return await _context.Users
                    .Include(p => p.Comments)
                    .Include(p => p.ArticleLikes)
                    .Include(p => p.CommentLikes)
                    .ToListAsync();
        }

        public void UpdateUserByRefreshToken(User user, string refreshToken, TimeSpan refreshTokenLifeTime)
        {
            user.RefreshTokens.Add(new RefreshToken { Token = refreshToken, Expires = DateTime.UtcNow.Add(refreshTokenLifeTime) });
            _context.Update(user);
        }
    }
}
