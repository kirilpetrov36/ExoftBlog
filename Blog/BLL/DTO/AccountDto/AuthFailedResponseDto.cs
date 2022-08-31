namespace Blog.BLL.DTO.AccountDto
{
    public class AuthFailedResponseDto : AuthenticationResultDto
    {
        public IEnumerable<string> Errors { get; set; }
        public int ErrorCode { get; set; }
    }
}
