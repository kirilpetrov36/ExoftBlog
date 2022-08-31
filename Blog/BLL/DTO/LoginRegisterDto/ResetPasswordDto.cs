using System.ComponentModel.DataAnnotations;

namespace Blog.BLL.DTO.LoginRegisterDto
{
    public class ResetPasswordDto
    {
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

        public string Code
        {
            get; set;
        }
    }
}
