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
        public DbSet<PhoneNumberModel> PhoneNumbers { get; set; }
        public DbSet<UserRelationModel> UserRelations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .HasMany(u => u.PhoneNumbers)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);
                

            // User to UserRelations relationship
            modelBuilder.Entity<UserRelationModel>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRelations)
                .HasForeignKey(ur => ur.UserId);


            // UserRelation to RelatedUser relationship
            modelBuilder.Entity<UserRelationModel>()
                .HasOne(ur => ur.RelatedUser)
                .WithMany()
                .HasForeignKey(ur => ur.RelatedUserId);


            // City to Users relationship
            modelBuilder.Entity<CityModel>()
                .HasMany(c => c.Users)
                .WithOne(u => u.City)
                .HasForeignKey(u => u.CityId);
               

        }
    }
}
