using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using turbin.sikker.core.Model;

namespace turbin.sikker.core.Configuration
{
    public static class UserConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserRole)
                .WithMany()
                .HasForeignKey(u => u.UserRoleId);

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => ur.Id);


        }
    }

    public static class ChecklistConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Checklist>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Checklist>()
                .HasMany(e => e.ChecklistTasks)
                .WithMany()
                .UsingEntity("ChecklistToTaskLink");

            modelBuilder.Entity<Checklist>()
                .HasOne(c => c.CreatedByUser)
                .WithMany()
                .HasForeignKey(c => c.CreatedBy);
        }
    }
}