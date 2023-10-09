using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO
{

    public class WorkflowInfo{
        public string Id { get; set; }
        public string Name { get; set; }
        public int CompletionTime { get; set; }
        public int HourlyRate { get; set; }
    }
    public class InvoiceBusDto
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

        [Required]
        public InvoiceStatus Status { get; set; }

        [Required]
        public ICollection<WorkflowInfo> Workflows { get; set; }


    }
}

