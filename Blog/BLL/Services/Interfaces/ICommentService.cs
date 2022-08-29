using Blog.BLL.DTO.CommentDto;
using Blog.BLL.DTO;
using Blog.BLL.DTO.LikeDto;

namespace Blog.BLL.Services.Interfaces
{
    public interface ICommentService
    {
        Task<ReadCommentDto> CreateCommentAsync(CreateCommentDto item, CancellationToken token = default);
        Task DeleteCommentAsync(long id, CancellationToken token = default);
        Task<ReadCommentDto> GetCommentAsync(long id, CancellationToken token = default);
        Task<ReadCommentLikesDto> GetLikesAsync(long id, CancellationToken token = default);
        Task<ReadCommentChildsDto> GetCommentChildsAsync(long id, CancellationToken token = default);
        Task<IEnumerable<ReadCommentDto>> GetCommentsAsync(CancellationToken token = default);
        Task<CreateCommentDto> UpdateCommentAsync(long id, CreateCommentDto item, CancellationToken token = default);
        Task<CreateCommentDto> ChangeCommentStateAsync(long id, ChangeStateDto item, CancellationToken token = default);
    }
}
