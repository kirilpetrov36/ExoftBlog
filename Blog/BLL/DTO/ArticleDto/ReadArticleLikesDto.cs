﻿using Blog.BLL.DTO.LikeDto;

namespace Blog.BLL.DTO.ArticleDto
{
    public class ReadArticleLikesDto
    {
        public List<ReadLikeDto>? CommentLikes { get; set; }
    }
}
