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
                Status = invoice.Status.ToString(),
                CreatedDate = invoice.CreatedDate,
                UpdatedDate = invoice.UpdatedDate,
                // ChecklistId = invoice.ChecklistId,
                // Checklist = invoice.Checklist,
                Receiver = invoice.Receiver,
                // ReceiverEmail = invoice.ReceiverEmail,
                Amount = invoice.Amount

            };
        }
    }
}