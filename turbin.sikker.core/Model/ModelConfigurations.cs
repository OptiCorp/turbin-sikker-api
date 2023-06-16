using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;

namespace turbin.sikker.core.Configuration
{
    public static class ModelConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserRole)
                .WithMany(ur => ur.Users)
                .HasForeignKey(u => u.UserRoleId);

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => ur.Id);

            modelBuilder.Entity<UserRole>()
                .HasMany(ur => ur.Users)
                .WithOne(u => u.UserRole)
                .HasForeignKey(u => u.UserRoleId);

            modelBuilder.Entity<User>()
                .ToTable("User");

            modelBuilder.Entity<UserRole>()
                .ToTable("UserRole");
        }
    }
}