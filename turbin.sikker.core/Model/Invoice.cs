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
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        public DateTime? CreatedDate { get; set; }
        [Required]
        public DateTime? UpdatedDate { get; set; }
        [Required]
        public DateTime? SentDate { get; set; }
        [Required]
        [EnumDataType(typeof(InvoiceStatus))]
        public InvoiceStatus? Status { get; set; }
        [Required]
        public string Sender { get; set; }
        [Required]
        public string Receiver { get; set; }
        [Required]
        public string ReceiverEmail { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public string PdfBlobLink { get; set; }
        [Required]
        public string ChecklistIds { get; set; }
        [Required]
        public string ChecklistNames { get; set; }
    }
}
