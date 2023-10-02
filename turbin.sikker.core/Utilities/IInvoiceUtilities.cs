using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;


namespace turbin.sikker.core.Utilities
{
public interface IInvoiceUtilities
    {
        bool IsValidStatus(string value);
        string GetInvoiceStatus(InvoiceStatus status);

        public InvoiceResponseDto InvoiceToResponseDto(Invoice? punch);
    }
}