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
        private readonly IConfiguration _config;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserSubscriptionService _userSubscriptionService;

        public AccountController(UserManager<User> userManager,
                                 IAccountService accountService,
                                 IConfiguration config,
                                 SignInManager<User> signInManager,
                                 IUserSubscriptionService userSubscriptionService)
        {
            _userManager = userManager;
            _accountService = accountService;
            _accountService.SetUserManager(_userManager);
            _config = config;
            _signInManager = signInManager;
            _userSubscriptionService = userSubscriptionService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model, CancellationToken token = default)
        {
            if (ModelState.IsValid)
            {
                (IdentityResult result, User user) = await _accountService.RegisterAsync(model);
                if (result.Succeeded)
                {
                    var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var uriBuilder = new UriBuilder(_config["ReturnPaths:ConfirmEmail"]);
                    var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                    query["token"] = emailToken;
                    query["userId"] = user.Id.ToString();
                    Console.WriteLine(user.Id.ToString());
                    uriBuilder.Query = query.ToString();
                    var urlString = uriBuilder.ToString();

                    EmailService emailService = new EmailService();
                    await emailService.SendEmailAsync(model.Email, "Confirm your account",
                        $"Підтвердьте реєcтрацію за посиланням: <a href='{urlString}'>link</a>");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return BadRequest(false);
                }
            }
            return Ok(true);
        }

        [HttpPost]
        [Route("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto model, CancellationToken token = default)
        {
            if (model.UserId == null || model.Token == null)
            {
                return BadRequest("Invalid userId or token");
            }
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return BadRequest("User doesn't exist");
            }
            var result = await _userManager.ConfirmEmailAsync(user, model.Token);
            if (result.Succeeded)
            {

                var refreshToken = await _userManager.GenerateUserTokenAsync(user, "MyApp", "RefreshToken");
                await _userManager.SetAuthenticationTokenAsync(user, "MyApp", "RefreshToken", refreshToken);


                AuthSucceededResponseDto response = new AuthSucceededResponseDto()
                {
                    Token = await _accountService.GetAccessTokenAsync(user),
                    RefreshToken = refreshToken,
                    Success = true,
                    UserFirstName = user.FirstName,
                    UserLastName = user.LastName,
                    UserId = user.Id,
                    IsAdmin = await _userManager.IsInRoleAsync(user, "Admin")
                };
                return Ok(response);
            }
            else
                return BadRequest("Account wasn't confirmed");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginViewModel, CancellationToken token = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.Select(x => x.Errors));
            }

            User user = await _userManager.FindByEmailAsync(loginViewModel.Email);
            if (user == null)
            {
                return BadRequest("Invalid email or user doesn't exist");
            }

            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (isEmailConfirmed)
            {
                AuthenticationResultDto loginResponse = await _accountService.LoginAsync(loginViewModel);
                return GetRegisterAuthResponse(loginResponse);
            }
            else
            {
                return BadRequest("Email is not confirmed");
            }
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenDto refreshTokenDto, CancellationToken token = default)
        {
            AuthenticationResultDto logoutResponse = await _accountService.LogoutAsync(refreshTokenDto);
            return GetRegisterAuthResponse(logoutResponse);
        }

        [HttpPost]
        [Route("refreshJwt")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshToken, CancellationToken token = default)
        {
            AuthenticationResultDto refreshTokenResponse = await _accountService.RefreshTokenAsync(refreshToken);

            if (!refreshTokenResponse.Success)
            {
                return Unauthorized(refreshTokenResponse as AuthFailedResponseDto);
            }
            return Ok(refreshTokenResponse as TokenResponseDto);
        }

        [HttpGet]
        [Authorize]
        [Route("SubscribeToUser/{UserToSubscribeId}")]
        public async Task<IActionResult> Subscribe(Guid UserToSubscribeId, CancellationToken token = default)
        {
            UserSubscriptionDto userSubscription = await _userSubscriptionService.CreateUserSubscriptionAsync(UserToSubscribeId);
            return Ok(userSubscription);
        }

        [HttpDelete]
        [Authorize]
        [Route("UnsubscribeFromUser/{UserToUnsubscribeId}")]
        public async Task<IActionResult> Unsubscribe(Guid UserToUnsubscribeId, CancellationToken token = default)
        {
            await _userSubscriptionService.DeleteUserSubscriptionAsync(UserToUnsubscribeId);
            return Ok();
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

        private IActionResult GetRegisterAuthResponse(AuthenticationResultDto response, CancellationToken token = default)
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
        public async Task<string> GetUserInfo(CancellationToken token = default)
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
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordByOldPasswordDto passwords, CancellationToken token = default)
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
        public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserDto value, CancellationToken token = default)
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
