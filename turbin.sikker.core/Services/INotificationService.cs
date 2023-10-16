using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Services
{
    public interface INotificationService
    {
        // Task<IEnumerable<InvoiceResponseDto>> GetAllInvoicesAsync();
        Task<IEnumerable<NotificationResponseDto>> GetAllNotificationsAsync();

        // Task<InvoiceResponseDto> GetInvoiceByIdAsync(string id);
        // Task<InvoiceResponseDto> GetInvoicePdfByInvoiceIdAsync(string id);
        // Task CreateInvoiceAsync(InvoiceCreateDto invoice);
        // Task UpdateInvoiceAsync(InvoiceUpdateDto invoice);
        // Task DeleteInvoiceAsync(string id);
    }
}
