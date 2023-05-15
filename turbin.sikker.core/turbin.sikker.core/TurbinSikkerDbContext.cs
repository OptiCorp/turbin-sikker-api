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

    }
}
