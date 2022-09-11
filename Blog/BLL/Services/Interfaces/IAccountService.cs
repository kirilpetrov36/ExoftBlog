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
        AuthenticationResultDto LogoutAsync(RefreshTokenDto registerViewModel, CancellationToken cancellationToken = default);
        Task<ReadUserDto> GetUserInfo(string id, CancellationToken token = default);
        Task<ReadUserDto> UpdateUserInfo(User user, UpdateUserDto newUser);
        Task<bool> ResetPassword(User user, ResetPasswordByOldPasswordDto passwords);
        string GetRefreshToken(User user);
        Task<string> GetAccessTokenAsync(User user);
        void SaveToken(User user, string refreshToken);
        Task<ReadUserDto> GetById(string userId);
        void SetUserManager(UserManager<User> user);
    }
}
