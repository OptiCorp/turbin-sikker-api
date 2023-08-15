using Microsoft.EntityFrameworkCore;
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
                .HasForeignKey(c => c.CreatedBy)
                .HasPrincipalKey(u => u.Id);
        }
    }

    public static class TaskConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChecklistTask>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<ChecklistTask>()
                .HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<Category>()
                .HasKey(c => c.Id);
        }
    }

    public static class PunchConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Punch>()
                .HasOne(p => p.CreatedByUser)
                .WithMany()
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }


}