using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Blog.BLL.DTO.LoginRegisterDto
{
    public class LoginDto
    {
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        //public string ReturnUrl { get; set; }

        //public IList<AuthenticationScheme> ExternalLogins { get; set; }
    }
}
