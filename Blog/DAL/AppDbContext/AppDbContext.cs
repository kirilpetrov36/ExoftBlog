using Microsoft.EntityFrameworkCore;
using Blog.DAL.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Blog.DAL.Entities
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.BuildPostModel();
            modelBuilder.BuildPostLikeModel();
            modelBuilder.BuildCommentLikeModel();
            modelBuilder.BuildCommentModel();
            modelBuilder.BuildUserModel();
            modelBuilder.BuildImageModel();
            modelBuilder.BuildRefreshTokenModel();
        }
    }
}
