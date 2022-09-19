using Blog.BLL.DTO.LikeDto;
using Blog.DAL.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace Blog.BLL.Services.Interfaces
{
    public interface IArticleLikeService
    {
        Task<ReadArticleLikeDto> CreateArticleLikeAsync(CreateArticleLikeDto item, CancellationToken token = default);
        Task<ReadArticleLikeDto> GetArticleLikeAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<ReadArticleLikeDto>> GetArticleLikesAsync(CancellationToken token = default);
        Task<ReadArticleLikeDto> PatchArticleLikeAsync(Guid id, JsonPatchDocument<ArticleLike> articleLikeUpdates, CancellationToken token = default);
    }

    public interface ICommentLikeService
    {
        Task<ReadCommentLikeDto> CreateCommentLikeAsync(CreateCommentLikeDto item, CancellationToken token = default);
        Task<ReadCommentLikeDto> GetCommentLikeAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<ReadCommentLikeDto>> GetCommentLikesAsync(CancellationToken token = default);
        Task<ReadCommentLikeDto> PatchCommentLikeAsync(Guid id, JsonPatchDocument<CommentLike> CommentLikeUpdates, CancellationToken token = default);
    }
}
