using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace turbin.sikker.core.Model
{
    public enum InvoiceStatus
    {
        [Display(Name = "Paid")]
        Paid,
        [Display(Name = "Unpaid")]
        Unpaid
    }

    public class Invoice
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime SentDate { get; set; }

        [EnumDataType(typeof(InvoiceStatus))]
        public InvoiceStatus Status { get; set; }

        public string Sender { get; set; }

        public string Receiver { get; set; }

        public float Amount { get; set; }

        public string PdfBlobLink { get; set; }

        public ICollection<Workflow>? Workflows { get;}
    }
}
