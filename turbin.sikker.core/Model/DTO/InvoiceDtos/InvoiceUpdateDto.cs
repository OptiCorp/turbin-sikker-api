using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO
{
    public class InvoiceUpdateDto
    {   
        [Required]
        public string? Id { get; set; }
    
        [Required]
        public string? Receiver { get; set; }

        [Required]
        public string? ReceiverEmail { get; set; }

        [Required]
        public int? Amount { get; set; }

        [Required]
        public string? Status { get; set; }
    }
}

