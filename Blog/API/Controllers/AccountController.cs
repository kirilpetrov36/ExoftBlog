using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Blog.BLL.DTO.LoginRegisterDto;
using Blog.BLL.DTO.AccountDto;
using Blog.BLL.DTO.UserDto;
using Blog.DAL.Entities;
using Blog.BLL.Services.ExternalServices;
using Blog.BLL.Services.Interfaces;
//using Blog.DAL.EF.Models;
using Blog.BLL.Constants;
//using System.Web.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAccountService _accountService;
        private readonly JwtSettingsDto _jwtSettings;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
                                            IAccountService accountService, JwtSettingsDto jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountService = accountService;
            _jwtSettings = jwtSettings;
            _accountService.SetUserManager(_userManager);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.Select(x => x.Errors));
            }
            AuthenticationResultDto registerResponse = await _accountService.RegisterAsync(registerViewModel);

            return GetRegisterAuthResponse(registerResponse);
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
        [Route("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshToken)
        {
            AuthenticationResultDto refreshTokenResponse = await _accountService.RefreshTokenAsync(refreshToken.RefreshToken);

            if (!refreshTokenResponse.Success)
            {
                return Unauthorized(refreshTokenResponse as AuthFailedResponseDto);
            }
            return Ok(refreshTokenResponse as TokenResponseDto);
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
        [Route("LoginExternal")]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl, CancellationToken cancellationToken = default)
        {
            if (ModelState.IsValid)
            {
                string redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
                Microsoft.AspNetCore.Authentication.AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
                return new ChallengeResult(provider, properties);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        [Route("ExternalLoginCallback")]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, CancellationToken cancellationToken = default)
        {
            //string identityExternalCookie = Request.Cookies["Identity.External"];//do we have the cookie?

            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return new RedirectResult(returnUrl);
            }

            // Sign in the user with this external login provider if the user already has a login.
            Microsoft.AspNetCore.Identity.SignInResult result =
                await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                AuthenticationResultDto response = await _accountService.ExternalLoginAsync(info);

                var okResponse = GetRegisterAuthResponse(response) as OkObjectResult;

                string token = ((AuthSucceededResponseDto)okResponse.Value).Token;
                string refreshToken = ((AuthSucceededResponseDto)okResponse.Value).RefreshToken;
                returnUrl += $"?token={token}&refreshToken={refreshToken}";
                return new RedirectResult(returnUrl);
            }

            if (result.IsLockedOut)
            {
                return new RedirectResult(returnUrl);
            }
            else
            {
                // If the user does not have an account, then create an account.
                AuthenticationResultDto response = await _accountService.ExternalRegisterAsync(info);

                // If the email already exists, redirect back to login
                if (response.GetType() == typeof(AuthFailedResponseDto) && (response as AuthFailedResponseDto).ErrorCode == StatusCodes.Status400BadRequest)
                {
                    return new RedirectResult(returnUrl + "?error=duplicate_email");
                }

                var okResponse = GetRegisterAuthResponse(response) as OkObjectResult;

                string token = ((AuthSucceededResponseDto)okResponse.Value).Token;
                string refreshToken = ((AuthSucceededResponseDto)okResponse.Value).RefreshToken;
                returnUrl += $"?token={token}&refreshToken={refreshToken}";
                return new RedirectResult(returnUrl);
            }
        }

        //// User forgot password, so reset link will be sent to email
        //[HttpPost]
        //[Route("Forgot")]
        //[AllowAnonymous]
        ///*[ValidateAntiForgeryToken]*/
        //public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model, CancellationToken cancellationToken = default)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        User user = null;
        //        user = await _userManager.FindByEmailAsync(model.Email);
        //        if (user != null)
        //        {
        //            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
        //            string callbackUrl = Url.Action("ResetPassword", "Account", new { email = user.Email, code = code }, protocol: HttpContext.Request.Scheme);
        //            EmailService emailService = new EmailService();
        //            // This if for testing only (while there is no email)
        //            // return Ok("The link to the password reset action: " + callbackUrl);
        //            await emailService.SendEmailAsync(model.Email, "Reset Password",
        //                $"Link to reset password: <a href='{callbackUrl}'>link</a>");
        //        }
        //        return Ok("The email with the password reset link has been sent.");
        //    }
        //    return BadRequest(ModelState);
        //}

        // Resetting user's password after clicking the email link
        [HttpPost]
        [Route("Reset")]
        [AllowAnonymous]
        /*[ValidateAntiForgeryToken]*/
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
