using Blog.BLL.DTO.LikeDto;

namespace Blog.BLL.DTO.ArticleDto
{
    public class ReadArticleLikesDto : BaseDto
    {
        public List<ReadLikeDto>? CommentLikes { get; set; }
    }
}
