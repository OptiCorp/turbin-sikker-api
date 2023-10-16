using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Services
{
    public interface IInvoiceService
    {
        Task<IEnumerable<InvoiceResponseDto>> GetAllInvoicesAsync();
        Task<InvoiceResponseDto> GetInvoiceByIdAsync(string id);
        Task<InvoiceResponseDto> GetInvoicePdfByInvoiceIdAsync(string id);
        Task<IEnumerable<InvoiceResponseDto>> GetAllInvoicePdfsAsync();
        Task CreateInvoiceAsync(InvoiceCreateDto invoice);
        Task UpdateInvoiceAsync(InvoiceUpdateDto invoice);
        Task DeleteInvoiceAsync(string id);
    }
}