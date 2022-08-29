using Microsoft.EntityFrameworkCore;
using Blog.DAL.Entities;

namespace Blog.DAL.Extensions
{
    public static class CommentExtension
    {
        public static void BuildCommentModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Comment>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Comment>()
                .Property(p => p.Text)
                .HasMaxLength(5000)
                .IsRequired();

            modelBuilder.Entity<Comment>()
                .HasOne(p => p.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<Comment>()
                .HasOne(p => p.User)
                .WithMany(p => p.Comments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<Comment>()
                .Property(p => p.UserId)
                .IsRequired();

            modelBuilder.Entity<Comment>()
                .HasOne(p => p.ParentComment)
                .WithMany(p => p.ChildComments)
                .HasForeignKey(p => p.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .Property(p => p.CreatedAt)
                .IsRequired();

            modelBuilder.Entity<Comment>()
                .Property(p => p.UpdatedAt)
                .IsRequired();

        }
    }
}
