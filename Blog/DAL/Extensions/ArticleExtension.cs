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
                .Property(p => p.Content)
                .IsRequired()
                .HasMaxLength(10000);

            modelBuilder.Entity<Article>()
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);
        }

    }
}
