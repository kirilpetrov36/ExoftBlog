using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Blog.BLL.DTO.LoginRegisterDto;
using Blog.BLL.DTO.AccountDto;
using Blog.BLL.DTO.UserDto;
using Blog.DAL.Entities;
using Blog.BLL.Services.ExternalServices;
using Blog.BLL.Services.Interfaces;
using Blog.BLL.Constants;

namespace  Blog.API.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IAccountService _accountService;

        public AccountController(UserManager<User> userManager, IAccountService accountService)
        {
            _userManager = userManager;
            _accountService = accountService;
            _accountService.SetUserManager(_userManager);
        }

        [HttpPost]
        [Route("register-by-email")]
        public async Task RegisterByEmail([FromBody]RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                MapperConfiguration config = new MapperConfiguration(config => config.CreateMap<RegisterDto, User>()
                .ForMember(x => x.UserName, x => x.MapFrom(m => m.Email)));
                Mapper mapper = new Mapper(config);
                User user = mapper.Map<User>(model);
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);               
                if (result.Succeeded)
                {
                    IdentityResult addToRoleUserResult = await _userManager.AddToRoleAsync(user, Roles.user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme
                    );
                    EmailService emailService = new EmailService();
                    await emailService.SendEmailAsync(model.Email, "Confirm your account",
                        $"Підтвердьте реєcтрацію за посиланням: <a href='{callbackUrl}'>link</a>");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
        }

        [HttpGet]
        [Route("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return null;
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                string refreshToken = _accountService.GetRefreshToken(user);
                _accountService.SaveToken(user, refreshToken);
                AuthSucceededResponseDto response =  new AuthSucceededResponseDto()
                {
                    Token = await _accountService.GetAccessTokenAsync(user),
                    RefreshToken = refreshToken,
                    Success = true,
                    UserFirstName = user.FirstName,
                    UserLastName = user.LastName
                };
                return Ok(response);
            }
            else
                return Ok("Account wasn't confirmed");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.Select(x => x.Errors));
            }
            AuthenticationResultDto loginResponse = await _accountService.LoginAsync(loginViewModel);

            return GetRegisterAuthResponse(loginResponse);
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout(RefreshTokenDto refreshTokenDto)
        {
            AuthenticationResultDto logoutResponse = _accountService.LogoutAsync(refreshTokenDto);
            return GetRegisterAuthResponse(logoutResponse);
        }

        private IActionResult GetRegisterAuthResponse(AuthenticationResultDto response)
        {
            if (!response.Success)
            {
                AuthFailedResponseDto failedResponse = (response as AuthFailedResponseDto);
                if (failedResponse.ErrorCode == StatusCodes.Status400BadRequest)
                {
                    return BadRequest(failedResponse);
                }
                return InternalServerError(failedResponse);
            }
            return Ok(response as AuthSucceededResponseDto);
        }

        [HttpGet]
        [Route("Me")]
        [Authorize]
        public async Task<ReadUserDto> GetUserInfo()
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
            string id = identity.Claims.SingleOrDefault(claim => claim.Type == Consts.Id).Value;
            ReadUserDto user =
                await _accountService.GetById(id);

            return user;
        }

        [HttpPut]
        [Route("resetByOldPassword")]
        [Authorize]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordByOldPasswordDto passwords)
        {
            if (ModelState.IsValid)
            {
                ClaimsIdentity identity = (ClaimsIdentity)User.Identity;

                string id = identity.Claims.SingleOrDefault(claim => claim.Type == Consts.Id).Value;

                User user =
                    await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                    bool result = await _accountService.ResetPassword(user, passwords);
                    if (result)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest(ErrorMessages.InvalidData);
                    }
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("update")]
        [Authorize]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserDto value)
        {
            if (ModelState.IsValid)
            {
                ClaimsIdentity identity = (ClaimsIdentity)User.Identity;

                string id = identity.Claims.SingleOrDefault(claim => claim.Type == Consts.Id).Value;

                User user =
                    await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                    ReadUserDto updatedUser =
                             await _accountService.UpdateUserInfo(user, value);

                    if (updatedUser == null)
                    {
                        return BadRequest(ErrorMessages.InvalidData);
                    }
                    else
                    {
                        return Ok(updatedUser);
                    }
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        private IActionResult InternalServerError(AuthFailedResponseDto failedResponse)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, failedResponse);
        }
    }
}
