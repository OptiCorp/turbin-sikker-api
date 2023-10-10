using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO
{
    public class InvoiceCreateDto
    {   
        [Required]
        [EmailAddress]
        public string Receiver { get; set; }

        [Required]
        public ICollection<string> WorkflowIds { get; set; }

        [Required]
        public int HourlyRate { get; set; }

    }
}

