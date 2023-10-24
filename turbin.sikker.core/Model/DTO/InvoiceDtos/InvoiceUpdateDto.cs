using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO
{
    public class InvoiceUpdateDto
    {
        [Required]
        public string? Id { get; set; }

        [Required]
        public string? Status { get; set; }

        public string? Message { get; set; }
    }
}

