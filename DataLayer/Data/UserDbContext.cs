using DataLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Data
{
    public class UserDbContext : IdentityDbContext<UserApplication>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReviewTag>()
                .HasKey(x => new { x.ReviewId, x.TagId });

            modelBuilder.Entity<ReviewTag>()
                .HasOne(r => r.Review)
                .WithMany(rt => rt.ReviewTags)
                .HasForeignKey(r => r.ReviewId);

            modelBuilder.Entity<ReviewTag>()
                .HasOne(t => t.Tag)
                .WithMany(rt => rt.ReviewTags)
                .HasForeignKey(t => t.TagId);
        }

        public DbSet<ReviewCategory> ReviewCategories { get; set; }
        public DbSet<Review> Reviews { get; set; } 
    }
}
