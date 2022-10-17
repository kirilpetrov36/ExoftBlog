namespace Blog.BLL.DTO.ArticleDto
{
    public class CreateArticleDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsVerified { get; set; }
    }
}
