using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;

namespace turbin.sikker.core
{
    public class TurbinSikkerDbContext: DbContext
    {
        public TurbinSikkerDbContext(DbContextOptions<TurbinSikkerDbContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }

        public DbSet<UserRole> UserRole { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<FormTask> FormTask { get; set; }

        public DbSet<Form> Form { get; set; }

        public DbSet<Upload> Upload { get; set; }

        public DbSet<Punch> Punch { get; set; }


    }
}
