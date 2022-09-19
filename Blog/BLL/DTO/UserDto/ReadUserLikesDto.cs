using Blog.BLL.DTO.LikeDto;

namespace Blog.BLL.DTO.UserDto
{
    public class ReadUserArticleLikesDto : ReadUserDto
    {    
        public List<ReadLikeDto>? ArticleLikes { get; set; }
    }

    public class ReadUserCommentLikesDto : ReadUserDto
    {
        public List<ReadLikeDto>? CommentLikes { get; set; }
    }
}
