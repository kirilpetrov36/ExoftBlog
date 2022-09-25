using Blog.BLL.DTO.ArticleDto;
using Blog.DAL.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace Blog.BLL.Services.Interfaces
{
    public interface IArticleService
    {
        Task<ReadArticleDto> CreateArticleAsync(CreateArticleDto item, CancellationToken token = default);
        Task DeleteArticleAsync(Guid id, CancellationToken token = default);
        Task<Article> GetArticleAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<ReadArticleDto>> GetMostCommentableArticlesAsync(CancellationToken token = default);
        Task<IEnumerable<ReadArticleDto>> GetMostLikeableArticlesAsync(CancellationToken token = default);
        Task<ReadArticleCommentsDto> GetArticleCommentsAsync(Guid id, CancellationToken token = default);
        Task<ReadArticleLikesDto> GetArticleLikesAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<ReadArticleDto>> GetArticlesAsync(CancellationToken token = default);
        Task<ReadArticleDto> UpdateArticleAsync(Guid id, CreateArticleDto item, CancellationToken token = default);
        Task<ReadArticleDto> PatchArticleAsync(Guid id, JsonPatchDocument<Article> articleUpdates, CancellationToken token = default);
    }
}
