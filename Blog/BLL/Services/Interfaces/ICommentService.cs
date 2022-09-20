using Blog.BLL.DTO.CommentDto;
using Blog.BLL.DTO;
using Blog.BLL.DTO.LikeDto;
using Blog.BLL.DTO.ArticleDto;
using Blog.DAL.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace Blog.BLL.Services.Interfaces
{
    public interface ICommentService
    {
        Task<ReadCommentDto> CreateCommentAsync(CreateCommentDto item, CancellationToken token = default);
        Task DeleteCommentAsync(Guid id, CancellationToken token = default);
        Task<ReadCommentDto> GetCommentAsync(Guid id, CancellationToken token = default);
        Task<ReadCommentLikesDto> GetLikesAsync(Guid id, CancellationToken token = default);
        Task<ReadCommentChildsDto> GetCommentChildsAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<ReadCommentDto>> GetCommentsAsync(CancellationToken token = default);
        Task<CreateCommentDto> UpdateCommentAsync(Guid id, CreateCommentDto item, CancellationToken token = default);
        Task<ReadCommentDto> PatchCommentAsync(Guid id, JsonPatchDocument<Comment> articleUpdates, CancellationToken token = default);
        Task<int?> GetArticleCommentsAmount(Guid ArticleId);
    }
}
