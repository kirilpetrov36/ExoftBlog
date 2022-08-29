using Blog.BLL.DTO.UserDto;
using Blog.BLL.DTO;

namespace Blog.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<ReadUserDto> CreateUserAsync(CreateUserDto item, CancellationToken token = default);
        Task DeleteUserAsync(long id, CancellationToken token = default);
        Task<ReadUserDto> GetUserAsync(long id, CancellationToken token = default);
        Task<ReadUserCommentsDto> GetUserCommentsAsync(long id, CancellationToken token = default);
        Task<ReadUserPostLikesDto> GetUserPostLikesAsync(long id, CancellationToken token = default);
        Task<ReadUserCommentLikesDto> GetUserCommentLikesAsync(long id, CancellationToken token = default);
        Task<IEnumerable<ReadUserDto>> GetUsersAsync(CancellationToken token = default);
        Task<CreateUserDto> UpdateUserAsync(long id, CreateUserDto item, CancellationToken token = default);
        Task<CreateUserDto> ChangeUserStateAsync(long id, ChangeStateDto item, CancellationToken token = default);
    }
}
