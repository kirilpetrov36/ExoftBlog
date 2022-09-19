namespace Blog.BLL.DTO.AccountDto
{
    public class AuthSucceededResponseDto : AuthenticationResultDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
    }
}
