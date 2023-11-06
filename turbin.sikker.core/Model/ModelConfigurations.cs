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
                .HasMany(u => u.Notifications)
                .WithOne(u => u.Receiver);
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
                .HasOne(c => c.Creator)
                .WithMany()
                .HasForeignKey(c => c.CreatorId)
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
                .HasOne(p => p.Creator)
                .WithMany()
                .HasForeignKey(p => p.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Punch>()
                .HasOne(c => c.Workflow)
                .WithMany()
                .HasForeignKey(c => c.WorkflowId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Punch>()
                .HasOne(c => c.ChecklistTask)
                .WithMany()
                .HasForeignKey(c => c.ChecklistTaskId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Punch>()
            .HasMany(e => e.Uploads)
            .WithOne(e => e.Punch);
        }
    }

    public static class WorkflowConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Workflow>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Workflow>()
                .HasOne(p => p.Checklist)
                .WithMany()
                .HasForeignKey(p => p.ChecklistId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Workflow>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Workflow>()
                .HasOne(p => p.Creator)
                .WithMany()
                .HasForeignKey(p => p.CreatorId)
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
                .WithMany(c => c.Uploads)
                .HasForeignKey(c => c.PunchId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }

    public static class InvoiceConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invoice>()
                .HasKey(c => c.Id);

        }
    }

    public static class NotificationConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Notification>()
                .HasOne(c => c.Receiver)
                .WithMany(c => c.Notifications)
                .HasForeignKey(c => c.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
