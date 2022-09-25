using Microsoft.EntityFrameworkCore;
using Blog.DAL.Entities;

namespace Blog.DAL.Extensions
{
    public static class ArticleFileExtension
    {
        public static void BuildArticleFileModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleFile>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<ArticleFile>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ArticleFile>()
                .Property(p => p.Url)
                .IsRequired();

            modelBuilder.Entity<ArticleFile>()
                .HasOne(p => p.Article)
                .WithMany(p => p.ArticleFiles)
                .HasForeignKey(p => p.ArticleId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
