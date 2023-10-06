using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO
{
    public class InvoiceResponseDto
    {   
        [Required]
        public string Id { get; set; }

        [Required]
        public string Sender { get; set; }

        [Required]
        public string Receiver { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime SentDate { get; set; }

        [Required]
        public DateTime? UpdatedDate { get; set; }

        [Required]
        public int? Amount { get; set; }

        [Required]
        public string PdfBlobLink { get; set; }

        // [Required]
        // public List<string> WorkflowIds { get; set; }
    }
}

