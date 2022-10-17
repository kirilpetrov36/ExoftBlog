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
                .HasOne(p => p.Article)
                .WithMany(p => p.Comments)
                .HasForeignKey(p => p.ArticleId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<Comment>()
                .HasOne(p => p.User)
                .WithMany(p => p.Comments)
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<Comment>()
                .Property(p => p.CreatedBy)
                .IsRequired();

            modelBuilder.Entity<Comment>()
                .HasOne(p => p.ParentComment)
                .WithMany(p => p.ChildComments)
                .HasForeignKey(p => p.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
