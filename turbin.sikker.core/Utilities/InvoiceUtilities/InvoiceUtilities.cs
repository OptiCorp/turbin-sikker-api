using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Utilities
{
    public class InvoiceUtilities : IInvoiceUtilities
    {
        public bool IsValidStatus(string value)
        {
            string lowerCaseValue = value.ToLower();
            return lowerCaseValue == "paid" || lowerCaseValue == "unpaid";
        }

        public string GetInvoiceStatus(InvoiceStatus status)
        {
            switch (status)
            {
                case InvoiceStatus.Paid:
                    return "Paid";
                case InvoiceStatus.Unpaid:
                    return "Unpaid";
                default:
                    return "";
            }
        }

        public InvoiceResponseDto InvoiceToResponseDto(Invoice? invoice, byte[]? bytes)
        {
            if (invoice == null)
            {
                return null;
            }
            if (bytes == null)
            {
                return new InvoiceResponseDto
                {
                    Id = invoice.Id,
                    Number = invoice.Number,
                    Title = invoice.Title,
                    Sender = invoice.Sender,
                    Receiver = invoice.Receiver,
                    Status = GetInvoiceStatus(invoice.Status),
                    CreatedDate = invoice.CreatedDate,
                    SentDate = invoice.SentDate,
                    UpdatedDate = invoice.UpdatedDate,
                    Amount = invoice.Amount,
                    PdfBlobLink = invoice.PdfBlobLink,
                    Workflows = invoice.Workflows,
                    Message = invoice.Message
                };
            }
            return new InvoiceResponseDto
            {
                Id = invoice.Id,
                Number = invoice.Number,
                Title = invoice.Title,
                Sender = invoice.Sender,
                Receiver = invoice.Receiver,
                Status = GetInvoiceStatus(invoice.Status),
                CreatedDate = invoice.CreatedDate,
                SentDate = invoice.SentDate,
                UpdatedDate = invoice.UpdatedDate,
                Amount = invoice.Amount,
                Pdf = bytes,
                PdfBlobLink = invoice.PdfBlobLink,
                Workflows = invoice.Workflows,
                Message = invoice.Message
            };
        }
    }
}
