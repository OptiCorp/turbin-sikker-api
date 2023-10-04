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
        public string? Id { get; set; }

        [Required]
        [EnumDataType(typeof(InvoiceStatus))]
        public InvoiceStatus Status { get; set; }
        
        [Required]
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        // [Required]
        // public string? ChecklistId { get; set; }

        public Checklist? Checklist { get; }

        [Required]
        public string? Receiver { get; set; }

        // [Required]
        // public string? ReceiverEmail { get; set; }

        [Required]
        public int? Amount { get; set; }
    }
}
