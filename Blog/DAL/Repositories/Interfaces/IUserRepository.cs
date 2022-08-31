using Blog.DAL.Entities;

namespace Blog.DAL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        void DeleteAllUserRefreshTokens(string refreshToken, CancellationToken cancellationToken = default);
        Task<User> GetUserByRefreshToken(string refreshToken, CancellationToken cancellationToken = default);
        void UpdateUserByRefreshToken(User user, string refreshToken, TimeSpan refreshTokenLifeTime);
        Task<IEnumerable<User>> GetListAsync(CancellationToken token = default);
        Task<User> GetAsync(string id, CancellationToken token = default);
        Task CreateAsync(User item, CancellationToken token = default);
        Task<User> UpdateAsync(User item, CancellationToken token = default);
        Task DeleteAsync(string id, CancellationToken token = default);

    }
}
