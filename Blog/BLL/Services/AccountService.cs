using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Blog.BLL.Constants;
using Blog.BLL.Services.Interfaces;
using Blog.DAL.Entities;
using Blog.BLL.DTO.AccountDto;
using Blog.DAL.UnitOfWork;
using Blog.BLL.DTO.LoginRegisterDto;
using Blog.BLL.DTO.UserDto;

namespace Blog.BLL.Services
{
    public class AccountService : IAccountService
    {
        private const string PLACEHOLDER_SURNAME = "Surname";
        private UserManager<User> _userManager;
        private readonly JwtSettingsDto _jwtSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        //private readonly IFileProvider _fileProvider;

        public AccountService(JwtSettingsDto jwtSettings, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _jwtSettings = jwtSettings;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            //_fileProvider = fileProvider;
        }

        public void SetUserManager(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        //public async Task<AuthenticationResultDto> RegisterAsync(RegisterDto registerViewModel, CancellationToken cancellationToken = default)
        //{
        //    MapperConfiguration config = new MapperConfiguration(config => config.CreateMap<RegisterDto, User>()
        //        .ForMember(x => x.UserName, x => x.MapFrom(m => m.Email)));
        //    Mapper mapper = new Mapper(config);
        //    User user = mapper.Map<User>(registerViewModel);
        //    try
        //    {
        //        IdentityResult signupUserResult = await _userManager.CreateAsync(user, registerViewModel.Password);

        //        if (signupUserResult.Succeeded)
        //        {
        //            IdentityResult addToRoleUserResult = await _userManager.AddToRoleAsync(user, Roles.user);
        //            string refreshToken = GetRefreshToken(user);
        //            SaveToken(user, refreshToken);
        //            return new AuthSucceededResponseDto
        //            {
        //                Token = await GetAccessTokenAsync(user),
        //                RefreshToken = refreshToken,
        //                Success = true,
        //                UserFirstName = user.FirstName,
        //                UserLastName = user.LastName
        //            };
        //        }
        //        return new AuthFailedResponseDto
        //        {
        //            ErrorCode = StatusCodes.Status400BadRequest,
        //            Errors = signupUserResult.Errors.Select(x => x.Description)
        //        };
        //    }
        //    catch (SqlException sqlExc)
        //    {
        //        return GetErrors(sqlExc);
        //    }
        //    catch (Exception ex)
        //    {
        //        return GetErrors(ex);
        //    }
        //}

        public async Task<AuthenticationResultDto> LoginAsync(LoginDto loginViewModel, CancellationToken cancellationToken = default)
        {
            try
            {
                User user = await _userManager.FindByEmailAsync(loginViewModel.Email);
                if (user == null)
                {
                    return new AuthFailedResponseDto
                    {
                        ErrorCode = StatusCodes.Status400BadRequest,
                        Errors = new[] { ErrorMessages.UserDoesntExist }
                    };
                }
                bool userHasValidPassword = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (!userHasValidPassword)
                {
                    return new AuthFailedResponseDto
                    {
                        ErrorCode = StatusCodes.Status400BadRequest,
                        Errors = new[] { ErrorMessages.NoUserWithThePassword }
                    };
                }
                string refreshToken = GetRefreshToken(user);
                SaveToken(user, refreshToken);
                return new AuthSucceededResponseDto
                {
                    Token = await GetAccessTokenAsync(user),
                    RefreshToken = refreshToken,
                    Success = true,
                    UserFirstName = user.FirstName,
                    UserLastName = user.LastName
                };
            }
            catch (SqlException sqlExc)
            {
                return GetErrors(sqlExc);
            }
            catch (Exception exc)
            {
                return GetErrors(exc);
            }
        }

        public AuthenticationResultDto LogoutAsync(RefreshTokenDto refreshToken, CancellationToken cancellationToken = default)
        {
            try
            {
                _unitOfWork.UserRepository.DeleteAllUserRefreshTokens(refreshToken.RefreshToken, cancellationToken);
                return new AuthSucceededResponseDto
                {
                    Success = true
                };
            }
            catch (NullReferenceException ex)
            {
                return new AuthFailedResponseDto
                {
                    Errors = new[] { ErrorMessages.NoUserWithTheRefreshToken },
                    ErrorCode = StatusCodes.Status400BadRequest
                };
            }
            catch (Exception ex)
            {
                return new AuthFailedResponseDto
                {
                    Errors = new[] { ex.Message },
                    ErrorCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        public async Task<AuthenticationResultDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            User user = await _unitOfWork.UserRepository.GetUserByRefreshToken(refreshToken, cancellationToken);
            if (user == null)
            {
                return new AuthFailedResponseDto
                {
                    Errors = new[] { ErrorMessages.IncorrectRefreshToken },
                    ErrorCode = StatusCodes.Status401Unauthorized
                };
            }
            RefreshToken approvedRefreshToken = user.RefreshTokens.Single(x => x.Token == refreshToken);
            if (approvedRefreshToken.IsExpired)
            {
                return new AuthFailedResponseDto
                {
                    Errors = new[] { ErrorMessages.RefreshTokenExpired },
                    ErrorCode = StatusCodes.Status401Unauthorized
                };
            }
            string jwtToken = await GetAccessTokenAsync(user);
            return new TokenResponseDto { Token = jwtToken, Success = true };
        }

        public async Task<ReadUserDto> GetUserInfo(string id, CancellationToken token = default)
        {
            User user = await _userManager.FindByIdAsync(id);
            ReadUserDto readUserDto = _mapper.Map<ReadUserDto>(user);
            return readUserDto;
        }

        private async Task<string> GetAccessTokenAsync(User user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            string userRole = (await _userManager.GetRolesAsync(user))[0];  //  need to add user role to user entity to avoid this shit
            SecurityTokenDescriptor accessTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(Claims.Id,user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(Claims.UserFirstName, user.FirstName),
                    new Claim(Claims.UserLastName, user.LastName),
                    new Claim(ClaimTypes.Role, userRole)
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.UtcNow.Add(_jwtSettings.RefreshTokenLifeTime)
            };
            SecurityToken jwt = tokenHandler.CreateToken(accessTokenDescriptor);
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return token;
        }

        private string GetRefreshToken(User user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            SecurityTokenDescriptor refreshTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())
                }),
                Expires = DateTime.UtcNow.Add(_jwtSettings.RefreshTokenLifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken jwt = tokenHandler.CreateToken(refreshTokenDescriptor);
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return token;
        }

        private AuthenticationResultDto GetErrors(dynamic exception)
        {
            List<string> list = new List<string>
            {
                exception.Message.ToString(),
                exception.GetType().ToString()
            };
            return new AuthFailedResponseDto
            {
                ErrorCode = StatusCodes.Status500InternalServerError,
                Errors = list
            };
        }

        private void SaveToken(User user, string refreshToken)
        {
            _unitOfWork.UserRepository.UpdateUserByRefreshToken(user, refreshToken, _jwtSettings.RefreshTokenLifeTime);
        }

        public async Task<ReadUserDto> UpdateUserInfo(User oldUser, UpdateUserDto updatedUser)
        {
            User modifiedUser = _mapper.Map(updatedUser, oldUser);

            IdentityResult result = await _userManager.UpdateAsync(modifiedUser);

            if (result.Succeeded)
            {
                User user =
                    await _unitOfWork.UserRepository.GetAsync(oldUser.Id);

                return _mapper.Map<ReadUserDto>(user);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> ResetPassword(User user, ResetPasswordByOldPasswordDto passwords)
        {
            IdentityResult result = await _userManager.ChangePasswordAsync(user, passwords.OldPassword, passwords.NewPassword);

            return result.Succeeded;
        }

        public async Task<ReadUserDto> GetById(string userId)
        {
            User user =
                await _unitOfWork.UserRepository.GetAsync(userId);

            return _mapper.Map<ReadUserDto>(user);
        }
    }
}