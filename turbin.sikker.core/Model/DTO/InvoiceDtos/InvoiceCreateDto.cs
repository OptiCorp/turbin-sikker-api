using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO
{
    public class InvoiceCreateDto
    {   
        // [Required]
        // public string? ChecklistId { get; set; }

        [Required]
        public string? Receiver { get; set; }

        // [Required]
        // public string? ReceiverEmail { get; set; }

        [Required]
        public int? Amount { get; set; }

    }
}

