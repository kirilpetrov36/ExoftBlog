namespace Blog.BLL.DTO.AccountDto
{
    public class JwtSettingsDto
    {
        public string Secret { get; set; }
        public TimeSpan AccessTokenLifeTime { get; set; }
        public TimeSpan RefreshTokenLifeTime { get; set; }
    }
}
