using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.DAL.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetListAsync(CancellationToken token = default);
        Task<T> GetAsync(Guid id, CancellationToken token = default);
        void CreateAsync(T item, CancellationToken token = default);
        T UpdateAsync(T item, CancellationToken token = default);
        Task DeleteAsync(Guid id, CancellationToken token = default);
    }
}
