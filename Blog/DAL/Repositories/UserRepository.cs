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
            return _context.Users
                //.Include(p => p.Logo)
                .SingleOrDefault(user => user.RefreshTokens.Any(rtokens => rtokens.Token == refreshToken));
        }

        public async Task<User> GetAsync(string id, CancellationToken token = default)
        {
            try
            {
                return await _context.Users
                    .Include(p => p.Comments)
                        .ThenInclude(p => p.Post)
                    .Include(p => p.PostLikes)
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
                    .Include(p => p.PostLikes)
                    .Include(p => p.CommentLikes)
                    .ToListAsync();
        }

        public void UpdateUserByRefreshToken(User user, string refreshToken, TimeSpan refreshTokenLifeTime)
        {
            user.RefreshTokens.Add(new RefreshToken { Token = refreshToken, Expires = DateTime.UtcNow.Add(refreshTokenLifeTime) });
            _context.Update(user);
            _context.SaveChanges();
        }

        public async Task CreateAsync(User item, CancellationToken token = default)
        {
            _context.Set<User>().Add(item);
        }

        public async Task<User> UpdateAsync(User item, CancellationToken token = default)
        {
            _context.Set<User>().Update(item);
            return item;
        }

        public async Task DeleteAsync(string id, CancellationToken token = default)
        {
            User entity = await _context.Set<User>().FindAsync(id);

            if (entity != null)
            {
                _context.Set<User>().Remove(entity);
            }
        }
    }
}
