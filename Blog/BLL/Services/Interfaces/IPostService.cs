using Blog.BLL.DTO.PostDto;
using Blog.BLL.DTO;

namespace Blog.BLL.Services.Interfaces
{
    public interface IPostService
    {
        Task<ReadPostDto> CreatePostAsync(CreatePostDto item, CancellationToken token = default);
        Task DeletePostAsync(long id, CancellationToken token = default);
        Task<ReadPostDto> GetPostAsync(long id, CancellationToken token = default);
        Task<IEnumerable<ReadPostDto>> GetMostCommentablePostsAsync(CancellationToken token = default);
        Task<IEnumerable<ReadPostDto>> GetMostLikeablePostsAsync(CancellationToken token = default);
        Task<ReadPostCommentsDto> GetPostCommentsAsync(long id, CancellationToken token = default);
        Task<ReadPostLikesDto> GetPostLikesAsync(long id, CancellationToken token = default);
        Task<IEnumerable<ReadPostDto>> GetPostsAsync(CancellationToken token = default);
        Task<CreatePostDto> UpdatePostAsync(long id, CreatePostDto item, CancellationToken token = default);
        Task<CreatePostDto> ChangePostStateAsync(long id, ChangeStateDto item, CancellationToken token = default);
    }
}
