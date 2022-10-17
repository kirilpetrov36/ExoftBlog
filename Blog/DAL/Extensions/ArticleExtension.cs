using Microsoft.EntityFrameworkCore;
using Blog.DAL.Entities;

namespace Blog.DAL.Extensions
{
    public static class ArticleExtension
    {
        public static void BuildArticleModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Article>()
               .Property(p => p.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<Article>()
                .Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Article>()
                .HasOne(p => p.User)
                .WithMany(p => p.Articles)
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Article>()
                .Property(p => p.Content)
                .IsRequired()
                .HasMaxLength(10000);

            modelBuilder.Entity<Article>()
                .Ignore(p => p.LikesAmount);

            modelBuilder.Entity<Article>()
                .Ignore(p => p.CommentsAmount);

            modelBuilder.Entity<Article>()
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);
        }

    }
}
