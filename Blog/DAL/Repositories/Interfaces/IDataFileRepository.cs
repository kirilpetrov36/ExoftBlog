using Blog.DAL.Entities;

namespace Blog.DAL.Repositories.Interfaces
{
    public interface IDataFileRepository : IRepository<DataFile>
    {
        void DeleteAsync(DataFile file, CancellationToken token = default);
    }
}
