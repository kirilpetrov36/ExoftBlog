using Blog.DAL.UnitOfWork;
using Blog.BLL.Services.Interfaces;
using Blog.BLL.Services;
using AutoMapper;
using Blog.BLL.Configurations.MapperConfig;
using Blog.BLL.Services.ExternalServices;

namespace Blog.API.Configurations
{
    public static class ServicesConfiguration
    {
        public static void RegisterReposAndServices(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IArticleService, ArticleService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IArticleLikeService, ArticleLikeService>();
            services.AddTransient<ICommentLikeService, CommentLikeService>();
            services.AddTransient<IFileService, FileService>();
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
