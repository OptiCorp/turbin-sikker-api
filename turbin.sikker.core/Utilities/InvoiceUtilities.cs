using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Utilities
{
public class InvoiceUtilities : IInvoiceUtilities
	{
       public bool IsValidStatus(string value)
        {
            string lowerCaseValue = value.ToLower();
            return lowerCaseValue == "Paid" || lowerCaseValue == "Unpaid";
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

        public InvoiceResponseDto InvoiceToResponseDto(Invoice? invoice)
        {
            return new InvoiceResponseDto
            {
                Id = invoice.Id,
                Sender = invoice.Sender,
                Receiver = invoice.Receiver,
                Status = invoice.Status.ToString(),
                CreatedDate = invoice.CreatedDate,
                SentDate = invoice.SentDate,
                UpdatedDate = invoice.UpdatedDate,
                Amount = invoice.Amount,
                PdfBlobLink = invoice.PdfBlobLink,
                Workflows = invoice.Workflows
            };
        }
    }
}