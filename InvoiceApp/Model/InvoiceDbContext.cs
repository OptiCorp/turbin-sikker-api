using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace InvoiceApp.Model
{
    public class InvoiceContext : DbContext
    {
        public InvoiceContext(DbContextOptions<InvoiceContext> options)
            : base(options)
        { }

        public DbSet<Invoice> Invoice { get; set; }
    }

    public class InvoiceContextFactory : IDesignTimeDbContextFactory<InvoiceContext>
    {
        public InvoiceContext CreateDbContext(string[] args)
        {
            string sqlConnectionString = Environment.GetEnvironmentVariable("InvoiceDbConnectionString");
            var optionsBuilder = new DbContextOptionsBuilder<InvoiceContext>();
            optionsBuilder.UseSqlServer(sqlConnectionString);

            return new InvoiceContext(optionsBuilder.Options);
        }
    }
}