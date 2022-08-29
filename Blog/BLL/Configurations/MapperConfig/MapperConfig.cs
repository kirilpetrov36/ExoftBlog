using AutoMapper;
using Blog.BLL.DTO.PostDto;
using Blog.BLL.DTO.UserDto;
using Blog.BLL.DTO.LikeDto;
using Blog.BLL.DTO.CommentDto;
using Blog.DAL.Entities;

namespace Blog.BLL.Configurations.MapperConfig
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Post, CreatePostDto>().ReverseMap();
            CreateMap<Post, ReadPostDto>().ReverseMap();
            CreateMap<Post, ReadPostCommentsDto>().ReverseMap();
            CreateMap<Post, ReadPostLikesDto>().ReverseMap();

            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<User, ReadUserDto>().ReverseMap();
            CreateMap<User, ReadUserCommentsDto>().ReverseMap();
            CreateMap<User, ReadUserPostLikesDto>().ReverseMap();
            CreateMap<User, ReadUserCommentLikesDto>().ReverseMap(); 


            CreateMap<Comment, CreateCommentDto>().ReverseMap();
            CreateMap<Comment, ReadCommentDto>().ReverseMap();
            CreateMap<Comment, UpdateCommentDto>().ReverseMap();
            CreateMap<Comment, ReadCommentLikesDto>().ReverseMap();
            CreateMap<Comment, ReadCommentChildsDto>().ReverseMap();

            CreateMap<PostLike, CreatePostLikeDto>().ReverseMap();
            CreateMap<PostLike, ReadPostLikeDto>().ReverseMap();

            CreateMap<CommentLike, CreateCommentLikeDto>().ReverseMap();
            CreateMap<CommentLike, ReadCommentLikeDto>().ReverseMap();
            CreateMap<CommentLike, ReadLikeDto>().ReverseMap();

        }
    }
}
