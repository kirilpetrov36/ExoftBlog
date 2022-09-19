namespace Blog.BLL.DTO.LikeDto
{
    public class CreateArticleLikeDto
    {
        public Guid UserId { get; set; }
        public Guid ArticleId { get; set; }
    }

    public class CreateCommentLikeDto
    {
        public Guid UserId { get; set; }
        public Guid CommentId { get; set; }
    }
}
