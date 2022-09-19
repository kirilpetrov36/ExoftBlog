using System.ComponentModel.DataAnnotations;

namespace Blog.BLL.DTO.LoginRegisterDto
{
    public class RegisterDto
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
