using System.ComponentModel.DataAnnotations;

namespace Blog.BLL.DTO.LoginRegisterDto
{
    public class ResetPasswordDto
    {
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Code { get; set; }
    }
}
