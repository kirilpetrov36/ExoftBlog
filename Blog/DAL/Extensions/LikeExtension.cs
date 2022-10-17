using Microsoft.EntityFrameworkCore;
using Blog.DAL.Entities;

namespace Blog.DAL.Extensions
{
    public static class ArticleLikeExtension
    {
        public static void BuildArticleLikeModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleLike>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<ArticleLike>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ArticleLike>()
                .HasOne(p => p.User)
                .WithMany(b => b.ArticleLikes)
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<ArticleLike>()
                .Property(p => p.CreatedBy)
                .IsRequired();

            modelBuilder.Entity<ArticleLike>()
                .HasOne(p => p.Article)
                .WithMany(b => b.Likes)
                .HasForeignKey(p => p.ArticleId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<ArticleLike>()
                .Property(p => p.ArticleId)
                .IsRequired();

            modelBuilder.Entity<ArticleLike>()
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);
        }
    }

    public static class СommentLikeExtension
    {
        public static void BuildCommentLikeModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommentLike>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<CommentLike>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<CommentLike>()
                .HasOne(p => p.User)
                .WithMany(b => b.CommentLikes)
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<CommentLike>()
                .Property(p => p.CreatedBy)
                .IsRequired();

            modelBuilder.Entity<CommentLike>()
                .HasOne(p => p.Comment)
                .WithMany(b => b.Likes)
                .HasForeignKey(p => p.CommentId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<CommentLike>()
                .Property(p => p.CommentId)
                .IsRequired();

            modelBuilder.Entity<CommentLike>()
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
