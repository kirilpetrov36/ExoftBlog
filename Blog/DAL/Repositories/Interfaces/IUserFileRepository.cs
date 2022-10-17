using Blog.DAL.Entities;

namespace Blog.DAL.Repositories.Interfaces
{
    public interface IUserFileRepository : IRepository<UserFile>
    {
        Task<List<UserFile>> GetUserFilesAsync(Guid userId, CancellationToken token = default);
        void DeleteAsync(UserFile file, CancellationToken token = default);
    }
}
