namespace Blog.BLL.DTO.LikeDto
{
    public class CreatePostLikeDto
    {
        public long UserId { get; set; }
        public long PostId { get; set; }
    }

    public class CreateCommentLikeDto
    {
        public long UserId { get; set; }
        public long CommentId { get; set; }
    }
}
