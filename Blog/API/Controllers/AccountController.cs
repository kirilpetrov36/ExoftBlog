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
using System.Web;

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
        public async Task<IActionResult> RegisterByEmail([FromBody]RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                (IdentityResult result, User user) = await _accountService.RegisterAsync(model);
                if (result.Succeeded)
                {
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
                    return BadRequest("Someting went wrong");
                }
            }
            return Ok("The email with the password reset link has been sent.");
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
                return BadRequest("Account wasn't confirmed");
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

        [HttpPost]
        [Route("Forgot")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model, CancellationToken cancellationToken = default)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    string code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    string callbackUrl = Url.Action("ResetPassword", "Account", new { email = user.Email, code = code }, protocol: HttpContext.Request.Scheme);
                    EmailService emailService = new EmailService();
                    await emailService.SendEmailAsync(model.Email, "Reset Password",
                        $"Link to reset password: <a href='{callbackUrl}'>link</a>");
                }
                return Ok("The email with the password reset link has been sent.");
            }
            return BadRequest(ModelState);
        }

        // Resetting user's password after clicking the email link
        [HttpPost]
        [Route("Reset")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("This email doesn't exist in the database.");
            }
            IdentityResult result = await _userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(model.Code), model.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(ModelState);
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
        public async Task<string> GetUserInfo()
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
            string id = identity.Claims.SingleOrDefault(claim => claim.Type == Consts.Id).Value;
            ReadUserDto user =
                await _accountService.GetById(new Guid(id));

            return id;
        }

        [HttpPut]
        [Route("resetByOldPassword")]
        [Authorize]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordByOldPasswordDto passwords)
        {
            if (ModelState.IsValid)
            {
                User user =
                    await _userManager.FindByIdAsync(_accountService.GetUserId().ToString());

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
        [Authorize(Roles = Roles.admin)]
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
