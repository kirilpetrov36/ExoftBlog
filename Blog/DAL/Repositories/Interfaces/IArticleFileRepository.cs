using Blog.DAL.Entities;

namespace Blog.DAL.Repositories.Interfaces
{
    public interface IArticleFileRepository : IRepository<ArticleFile>
    {
        void DeleteAsync(ArticleFile file, CancellationToken token = default);
    }
}
