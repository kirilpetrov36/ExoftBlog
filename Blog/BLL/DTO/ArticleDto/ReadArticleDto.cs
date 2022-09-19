namespace Blog.BLL.DTO.ArticleDto
{
    public class ReadArticleDto : BaseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsDeleted { get; set; }
    }
}
