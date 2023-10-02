using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace InvoiceApp
{
    public class InvoiceContext : DbContext
    {
        public InvoiceContext(DbContextOptions<InvoiceContext> options)
            : base(options)
        { }

        public DbSet<Invoice> Invoice { get; set; }
    }

    public enum InvoiceStatus
    {
        [Display(Name = "Active")]
        Active,
        [Display(Name = "Inactive")]
        Inactive
    }

    public class Invoice
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public DateTime SentDate { get; set; }

        [EnumDataType(typeof(InvoiceStatus))]
        public InvoiceStatus Status { get; set; }

        public string Sender { get; set; }

        public string Reciever { get; set; }

        public int Amount { get; set; }

        public string PdfBlobLink { get; set; }
    }

    public class InvoiceDto
    {
        public string Sender { get; set; }

        public string Reciever { get; set; }

        public int Amount { get; set; }
    }

    public class InvoiceContextFactory : IDesignTimeDbContextFactory<InvoiceContext>
    {
        public InvoiceContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<InvoiceContext>();
            optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("InvoiceDbConnectionString"));

            return new InvoiceContext(optionsBuilder.Options);
        }
    }
}