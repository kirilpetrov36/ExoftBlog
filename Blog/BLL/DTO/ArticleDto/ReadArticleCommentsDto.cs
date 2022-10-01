using Blog.BLL.DTO.CommentDto;

namespace Blog.BLL.DTO.ArticleDto
{
    public class ReadArticleCommentsDto
    {
        public List<ReadCommentDto>? Comments { get; set; }
    }
}
