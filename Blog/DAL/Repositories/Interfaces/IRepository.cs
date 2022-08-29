using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.DAL.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetListAsync(CancellationToken token = default);
        Task<T> GetAsync(long id, CancellationToken token = default);
        Task CreateAsync(T item, CancellationToken token = default);
        Task<T> UpdateAsync(T item, CancellationToken token = default);
        Task DeleteAsync(long id, CancellationToken token = default);
    }
}
