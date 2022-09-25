using Microsoft.EntityFrameworkCore;
using Blog.DAL.Entities;

namespace Blog.DAL.Extensions
{
    public static class DataFileExtension
    {
        public static void BuildDataFileModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataFile>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<DataFile>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<DataFile>()
                .Property(p => p.Url)
                .IsRequired();

            modelBuilder.Entity<DataFile>()
                .HasOne(p => p.Article)
                .WithMany(p => p.DataFiles)
                .HasForeignKey(p => p.ArticleId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
