﻿using Microsoft.EntityFrameworkCore;
using Blog.DAL.Entities;

namespace Blog.DAL.Extensions
{
    public static class UserExtension
    {
        public static void BuildUserModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<User>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>()
                .Property(p => p.Name)
                .HasMaxLength(32)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(p => p.CreatedAt)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(p => p.UpdatedAt)
                .IsRequired();
        }
    }
}
