namespace Blog.BLL.DTO.LikeDto
{
    public class CreateArticleLikeDto
    {
        public Guid ArticleId { get; set; }
    }

    public class CreateCommentLikeDto
    {
        public Guid CommentId { get; set; }
    }
}
