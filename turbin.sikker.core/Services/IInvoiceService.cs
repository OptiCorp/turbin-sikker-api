
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Services
{
    public interface IInvoiceService
    {
        Task<IEnumerable<InvoiceResponseDto>> GetAllInvoicesAsync();
        Task<InvoiceResponseDto> GetInvoiceByIdAsync(string id);
        // Task<InvoiceResponseDto> GetInvoiceByChecklistIdAsync(string checklistId);
        Task<string> CreateInvoiceAsync(InvoiceCreateDto invoice);
        Task UpdateInvoiceAsync(InvoiceUpdateDto invoice);
        Task DeleteInvoiceAsync(string id);
    }
}