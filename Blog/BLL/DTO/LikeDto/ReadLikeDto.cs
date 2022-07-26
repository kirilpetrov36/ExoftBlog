﻿using Blog.BLL.DTO.ArticleDto;
using Blog.BLL.DTO.UserDto;
using Blog.BLL.DTO.CommentDto;

namespace Blog.BLL.DTO.LikeDto
{
    public class ReadLikeDto : BaseDto
    {
        public bool IsDeleted { get; set; }
    }

    public class ReadArticleLikeDto : ReadLikeDto
    {     
        public Guid ArticleId { get; set; }
    }

    public class ReadCommentLikeDto : ReadLikeDto
    {
        public Guid CommentId { get; set; }
    }
}
