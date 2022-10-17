namespace Blog.BLL.DTO.LoginRegisterDto
{
    public class RefreshTokenDto
    {
        public Guid UserId { get; set; }
        public string RefreshToken { get; set; }
    }
}
