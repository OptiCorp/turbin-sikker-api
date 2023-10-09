using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO
{
    public class InvoiceCreateDto
    {   
        [Required]
        public string Sender { get; set; }

        [Required]
        public string Receiver { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime SentDate { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public string PdfBlobLink { get; set; }
    }
}

