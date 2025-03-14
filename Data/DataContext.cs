using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using UserProfileAPI.Models;

namespace UserProfileAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<CityModel> Cities { get; set; }
        public DbSet<UserRelationModel> UserRelations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRelationModel>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRelations)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserRelationModel>()
                .HasOne(ur => ur.RelatedUser)
                .WithMany()
                .HasForeignKey(ur => ur.RelatedUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserModel>()
               .HasOne(u => u.City)
               .WithMany(c => c.Users)
               .HasForeignKey(u => u.CityId)
               .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
