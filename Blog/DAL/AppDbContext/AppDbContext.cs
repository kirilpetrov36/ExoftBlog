using Microsoft.EntityFrameworkCore;
using Blog.DAL.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Blog.DAL.Entities
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleLike> ArticleLikes { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ArticleFile> ArticleFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.BuildArticleModel();
            modelBuilder.BuildArticleLikeModel();
            modelBuilder.BuildCommentLikeModel();
            modelBuilder.BuildCommentModel();
            modelBuilder.BuildUserModel();
            modelBuilder.BuildArticleFileModel();
            modelBuilder.BuildRefreshTokenModel();
            base.OnModelCreating(modelBuilder);
        }
    }
}
