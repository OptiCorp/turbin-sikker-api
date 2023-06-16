using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Configuration;
using turbin.sikker.core.Model;

namespace turbin.sikker.core
{
    public class TurbinSikkerDbContext: DbContext
    {
        public TurbinSikkerDbContext(DbContextOptions<TurbinSikkerDbContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }

        public DbSet<UserRole> User_Role { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<ChecklistTask> Checklist_Task { get; set; }

        public DbSet<Checklist> Checklist { get; set; }

        public DbSet<Upload> Upload { get; set; }

        public DbSet<Punch> Punch { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Call the Configure method from ModelConfigurations class
            ModelConfigurations.Configure(modelBuilder);
        }

    }
}
