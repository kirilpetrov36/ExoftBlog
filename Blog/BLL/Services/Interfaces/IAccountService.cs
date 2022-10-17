using Blog.BLL.DTO.AccountDto;
using Blog.BLL.DTO.UserDto;
using Blog.BLL.DTO.LoginRegisterDto;
using Blog.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace Blog.BLL.Services.Interfaces
{
    public interface IAccountService
    {
        Task<AuthenticationResultDto> LoginAsync(LoginDto registerViewModel, CancellationToken cancellationToken = default);
        Task<Tuple<IdentityResult, User>> RegisterAsync(RegisterDto model, CancellationToken cancellationToken = default);
        Task<AuthenticationResultDto> LogoutAsync(RefreshTokenDto registerViewModel, CancellationToken cancellationToken = default);
        //Task<ReadUserDto> GetUserInfo(string id, CancellationToken token = default);
        Task<AuthenticationResultDto> RefreshTokenAsync(RefreshTokenDto refreshToken, CancellationToken cancellationToken = default);
        Guid GetUserId();
        Task<ReadUserDto> UpdateUserInfo(User user, UpdateUserDto newUser);
        Task<bool> ResetPassword(User user, ResetPasswordByOldPasswordDto passwords);
        //string GetRefreshToken(User user);
        Task<string> GetAccessTokenAsync(User user);
        //void SaveToken(User user, string refreshToken);
        Task<ReadUserDto> GetById(Guid userId);
        void SetUserManager(UserManager<User> user);
    }
}
