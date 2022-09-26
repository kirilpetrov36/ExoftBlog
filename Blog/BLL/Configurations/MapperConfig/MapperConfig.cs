using AutoMapper;
using Blog.BLL.DTO.ArticleDto;
using Blog.BLL.DTO.UserDto;
using Blog.BLL.DTO.LikeDto;
using Blog.BLL.DTO.CommentDto;
using Blog.DAL.Entities;
using Blog.BLL.DTO.LoginRegisterDto;
using Blog.BLL.DTO.ArticleFileDto;

namespace Blog.BLL.Configurations.MapperConfig
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Article, CreateArticleDto>().ReverseMap();
            CreateMap<Article, ReadArticleDto>().ReverseMap();
            CreateMap<Article, ReadArticleCommentsDto>().ReverseMap();
            CreateMap<Article, ReadArticleLikesDto>().ReverseMap();
            CreateMap<Article, ReadFullArticleDto>().ReverseMap();

            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<User, ReadUserDto>().ReverseMap();
            CreateMap<User, ReadUserCommentsDto>().ReverseMap();
            CreateMap<User, ReadUserArticleLikesDto>().ReverseMap();
            CreateMap<User, ReadUserCommentLikesDto>().ReverseMap(); 

            CreateMap<Comment, CreateCommentDto>().ReverseMap();
            CreateMap<Comment, ReadCommentDto>().ReverseMap();
            CreateMap<Comment, UpdateCommentDto>().ReverseMap();
            CreateMap<Comment, ReadCommentLikesDto>().ReverseMap();
            CreateMap<Comment, ReadCommentChildsDto>().ReverseMap();

            CreateMap<ArticleLike, CreateArticleLikeDto>().ReverseMap();
            CreateMap<ArticleLike, ReadArticleLikeDto>().ReverseMap();

            CreateMap<CommentLike, CreateCommentLikeDto>().ReverseMap();
            CreateMap<CommentLike, ReadCommentLikeDto>().ReverseMap();

            CreateMap<RegisterDto, User>()
                .ForMember(x => x.UserName, x => x.MapFrom(m => m.Email));

            CreateMap<ArticleFile, ReadArticleFileDto>().ReverseMap();
        }
    }
}
