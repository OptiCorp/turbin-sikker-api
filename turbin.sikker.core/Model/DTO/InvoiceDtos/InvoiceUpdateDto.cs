using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO
{
    public class InvoiceUpdateDto
    {   
        [Required]
        public string? Id { get; set; }
    
        public string? Receiver { get; set; }

        // public string? ReceiverEmail { get; set; }

        public int? Amount { get; set; }

        public string? Status { get; set; }
    }
}

