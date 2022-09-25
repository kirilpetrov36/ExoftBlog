using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(JwtSettingsDto jwtSettings, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _jwtSettings = jwtSettings;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetUserManager(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Tuple<IdentityResult, User>> RegisterAsync(RegisterDto model, CancellationToken cancellationToken = default)
        {
            User user = _mapper.Map<User>(model);
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                IdentityResult addToRoleUserResult = await _userManager.AddToRoleAsync(user, Roles.user);
                return Tuple.Create(addToRoleUserResult, user);
            }
            return Tuple.Create(result, user);
        }

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

        public Guid GetUserId()
        {
            if (_httpContextAccessor != null)
            {
                ClaimsIdentity identity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
                string userId = identity.Claims.SingleOrDefault(claim => claim.Type == Consts.Id).Value;
                return new Guid(userId);
            }
            else
            {
                return Guid.Empty;
            }
            
        }

        public async Task<string> GetAccessTokenAsync(User user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            string userRole = (await _userManager.GetRolesAsync(user))[0];  //  need to add user role to user entity to avoid this shit
            SecurityTokenDescriptor accessTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(Claims.Id,user.Id.ToString()),
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

        public string GetRefreshToken(User user)
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

        public void SaveToken(User user, string refreshToken)
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

        public async Task<ReadUserDto> GetById(Guid userId)
        {
            User user = await _unitOfWork.UserRepository.GetAsync(userId);

            return _mapper.Map<ReadUserDto>(user);
        }
    }
}