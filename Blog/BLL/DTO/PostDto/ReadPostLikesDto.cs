using Blog.BLL.DTO.LikeDto;

namespace Blog.BLL.DTO.PostDto
{
    public class ReadPostLikesDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<ReadLikeDto>? CommentLikes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
