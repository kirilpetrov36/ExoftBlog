using Blog.BLL.DTO.UserDto;

namespace Blog.BLL.Services.Interfaces
{
    public interface IUserSubscriptionService
    {
        Task<UserSubscriptionDto> CreateUserSubscriptionAsync(Guid userToSubscribeId, CancellationToken token = default);
        Task DeleteUserSubscriptionAsync(Guid userToSubscribeId, CancellationToken token = default);
    }
}
