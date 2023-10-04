using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO
{
    public class InvoiceResponseDto
    {   
        public string? Id { get; set; }

        [EnumDataType(typeof(InvoiceStatus))]
        public string? Status { get; set; }
        
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        // public string? ChecklistId { get; set; }

        // public Checklist? Checklist { get; set; }

        public string? Receiver { get; set; }

        // public string? ReceiverEmail { get; set; }

        public int? Amount { get; set; }

    }
}

