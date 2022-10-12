using Blog.DAL.Entities;

namespace Blog.DAL.Repositories.Interfaces
{
    public interface IUserFileRepository : IRepository<UserFile>
    {
        void DeleteAsync(UserFile file, CancellationToken token = default);
    }
}
