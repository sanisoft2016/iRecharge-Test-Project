using iRechargeTestProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace iRechargeTestProject.Infrastructure.Data
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
       : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.UserName)
                .HasMaxLength(50);
            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.NormalizedUserName)
                .HasMaxLength(50);

            modelBuilder.Entity<ApplicationUser>()
               .Property(u => u.Email)
               .HasMaxLength(50);

            modelBuilder.Entity<ApplicationUser>()
               .Property(u => u.NormalizedEmail)
               .HasMaxLength(50);

            modelBuilder.Entity<IdentityRole>()
                .Property(r => r.Name)
                .HasMaxLength(20);

            modelBuilder.Entity<IdentityRole>()
               .Property(r => r.NormalizedName)
               .HasMaxLength(20);

            modelBuilder.Entity<IdentityRole>()
               .Property(r => r.Id)
               .HasMaxLength(40);

            modelBuilder.Entity<IdentityUserRole<string>>()
              .Property(r => r.UserId)
              .HasMaxLength(40);
            modelBuilder.Entity<IdentityUserRole<string>>()
             .Property(r => r.RoleId)
             .HasMaxLength(40);


            modelBuilder.Entity<IdentityUserRole<string>>()
               .HasIndex(p => p.UserId);

            modelBuilder.Entity<IdentityUserRole<string>>()
               .HasIndex(p => p.RoleId);

            modelBuilder.Entity<IdentityRole>()
                .HasIndex(p => p.Name)
               .IsUnique();

            modelBuilder.Entity<ApplicationUser>()
              .HasIndex(p => p.UserName)
              .IsUnique();

            modelBuilder.Entity<ApplicationUser>()
              .HasIndex(p => p.PhoneNumber)
              .IsUnique();
        }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        //public DbSet<Log> Logs { get; set; }
    }
}
