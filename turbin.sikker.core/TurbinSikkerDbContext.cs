﻿using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Configuration;
using turbin.sikker.core.Model;

namespace turbin.sikker.core
{
    public class TurbinSikkerDbContext : DbContext
    {
        public TurbinSikkerDbContext(DbContextOptions<TurbinSikkerDbContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<ChecklistTask> Checklist_Task { get; set; }

        public DbSet<Checklist> Checklist { get; set; }

        public DbSet<Upload> Upload { get; set; }

        public DbSet<Punch> Punch { get; set; }

        public DbSet<Workflow> Workflow { get; set; }

        public DbSet<TaskInfo> TaskInfo { get; set; }

        public DbSet<Invoice> Invoice { get; set; }

        public DbSet<Notification> Notification { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Call the Configure method from ModelConfigurations class
            UserConfigurations.Configure(modelBuilder);
            ChecklistConfigurations.Configure(modelBuilder);
            TaskConfigurations.Configure(modelBuilder);
            PunchConfigurations.Configure(modelBuilder);
            WorkflowConfigurations.Configure(modelBuilder);
            UploadConfigurations.Configure(modelBuilder);
            InvoiceConfigurations.Configure(modelBuilder);
            NotificationConfigurations.Configure(modelBuilder);
        }

    }
}
