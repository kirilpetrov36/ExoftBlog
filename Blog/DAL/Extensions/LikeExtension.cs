using Microsoft.EntityFrameworkCore;
using Blog.DAL.Entities;

namespace Blog.DAL.Extensions
{
    public static class PostLikeExtension
    {
        public static void BuildPostLikeModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostLike>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<PostLike>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<PostLike>()
                .HasOne(p => p.User)
                .WithMany(b => b.PostLikes)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<PostLike>()
                .Property(p => p.UserId)
                .IsRequired();

            modelBuilder.Entity<PostLike>()
                .HasOne(p => p.Post)
                .WithMany(b => b.Likes)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<PostLike>()
                .Property(p => p.PostId)
                .IsRequired();

            modelBuilder.Entity<PostLike>()
                .Property(p => p.CreatedAt)
                .IsRequired();

            modelBuilder.Entity<PostLike>()
                .Property(p => p.UpdatedAt)
                .IsRequired();

            modelBuilder.Entity<PostLike>()
                .Property(p => p.isEnable)
                .HasDefaultValue(true);

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
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<CommentLike>()
                .Property(p => p.UserId)
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
                .Property(p => p.CreatedAt)
                .IsRequired();

            modelBuilder.Entity<CommentLike>()
                .Property(p => p.UpdatedAt)
                .IsRequired();

            modelBuilder.Entity<CommentLike>()
                .Property(p => p.isEnable)
                .HasDefaultValue(true);

        }
    }
}
