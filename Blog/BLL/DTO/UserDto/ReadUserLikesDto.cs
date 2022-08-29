using Blog.BLL.DTO.LikeDto;

namespace Blog.BLL.DTO.UserDto
{
    public class ReadUserPostLikesDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<ReadLikeDto>? PostLikes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ReadUserCommentLikesDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<ReadLikeDto>? CommentLikes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
