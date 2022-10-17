namespace Blog.BLL.DTO.AccountDto
{
    public class AuthSucceededResponseDto : AuthenticationResultDto
    {
        public Guid UserId { get; set; }
        public string? AvatarUrl { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public bool IsAdmin { get; set; }
        
    }
}
