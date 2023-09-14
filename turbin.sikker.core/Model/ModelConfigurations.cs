using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Migrations;
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
                .HasKey(c => c.Id);

            modelBuilder.Entity<Punch>()
                .HasOne(p => p.CreatedByUser)
                .WithMany()
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Punch>()
                .HasOne(c => c.ChecklistWorkflow)
                .WithMany()
                .HasForeignKey(c => c.ChecklistWorkflowId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Punch>()
                .HasOne(c => c.ChecklistTask)
                .WithMany()
                .HasForeignKey(c => c.ChecklistTaskId)
                .OnDelete(DeleteBehavior.NoAction);

                modelBuilder.Entity<Punch>()
                .HasMany(e => e.Uploads)
                .WithOne();
        }
    }

    public static class WorkflowConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChecklistWorkflow>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<ChecklistWorkflow>()
                .HasOne(p => p.Checklist)
                .WithMany()
                .HasForeignKey(p => p.ChecklistId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ChecklistWorkflow>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ChecklistWorkflow>()
                .HasOne(p => p.Creator)
                .WithMany()
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

    public static class UploadConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Upload>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Upload>()
                .HasOne(c => c.Punch)
                .WithMany()
                .HasForeignKey(c => c.PunchId)
                .OnDelete(DeleteBehavior.NoAction);
            
        }
    }


}