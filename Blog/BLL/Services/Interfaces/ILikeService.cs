using Blog.BLL.DTO.LikeDto;
using Blog.BLL.DTO;

namespace Blog.BLL.Services.Interfaces
{
    public interface IPostLikeService
    {
        Task<ReadPostLikeDto> CreatePostLikeAsync(CreatePostLikeDto item, CancellationToken token = default);
        Task<ReadPostLikeDto> GetPostLikeAsync(long id, CancellationToken token = default);
        Task<IEnumerable<ReadPostLikeDto>> GetPostLikesAsync(CancellationToken token = default);
        Task<CreatePostLikeDto> ChangePostLikeStateAsync(long id, ChangeStateDto item, CancellationToken token = default);
    }

    public interface ICommentLikeService
    {
        Task<ReadCommentLikeDto> CreateCommentLikeAsync(CreateCommentLikeDto item, CancellationToken token = default);
        Task<ReadCommentLikeDto> GetCommentLikeAsync(long id, CancellationToken token = default);
        Task<IEnumerable<ReadCommentLikeDto>> GetCommentLikesAsync(CancellationToken token = default);
        Task<CreateCommentLikeDto> ChangeCommentLikeStateAsync(long id, ChangeStateDto item, CancellationToken token = default);
    }
}
