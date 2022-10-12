namespace Blog.DAL.Entities
{
    public class Article : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<ArticleLike>? Likes { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<ArticleFile>? ArticleFiles { get; set; }  
        public bool IsDeleted { get; set; }
        public bool IsVerified { get; set; }

    }
}
