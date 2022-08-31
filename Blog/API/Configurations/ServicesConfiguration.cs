using Blog.DAL.UnitOfWork;
using Blog.BLL.Services.Interfaces;
using Blog.BLL.Services;
using AutoMapper;
using Blog.BLL.Configurations.MapperConfig;

namespace Blog.API.Configurations
{
    public static class ServicesConfiguration
    {
        public static void RegisterReposAndServices(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IPostLikeService, PostLikeService>();
            services.AddTransient<ICommentLikeService, CommentLikeService>();
        }

        public static void RegisterMap(this IServiceCollection services)
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(c =>
            {
                c.AddProfile<MapperConfig>();
                //c.AddProfile<MapperConfigApi>();
            });

            services.AddSingleton(s => mapperConfiguration.CreateMapper());
        }
    }
}
