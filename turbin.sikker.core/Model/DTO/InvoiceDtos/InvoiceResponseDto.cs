
namespace turbin.sikker.core.Model.DTO
{
    public class InvoiceResponseDto
    {
        public string Id { get; set; }

        public int Number { get; set; }

        public string? Title { get; set; }

        public string Sender { get; set; }

        public string Receiver { get; set; }

        public string Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime SentDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public float? Amount { get; set; }

        public byte[]? Pdf { get; set; }

        public string PdfBlobLink { get; set; }

        public ICollection<Workflow>? Workflows { get; set; }

        public string? Message { get; set; }
    }
}

