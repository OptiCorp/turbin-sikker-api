using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO
{
    public class InvoiceSendDto
    {   
        [Required]
        public string Receiver { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public ICollection<WorkflowInfo> Workflows { get; set; }
    }
}

