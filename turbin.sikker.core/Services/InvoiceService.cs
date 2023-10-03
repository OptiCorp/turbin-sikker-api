using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Utilities;

namespace turbin.sikker.core.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly TurbinSikkerDbContext _context;

        private readonly IInvoiceUtilities _invoiceUtilities;

        public InvoiceService(TurbinSikkerDbContext context, IInvoiceUtilities invoiceUtilities)
        {
            _context = context;
            _invoiceUtilities = invoiceUtilities;
        }

        public async Task<IEnumerable<InvoiceResponseDto>> GetAllInvoicesAsync()
        {
            return await _context.Invoice
                            .Include(p => p.Checklist)
                            .OrderByDescending(c => c.CreatedDate)
                            .Select(p => _invoiceUtilities.InvoiceToResponseDto(p))
                            .ToListAsync();
        }

        public async Task<InvoiceResponseDto> GetInvoiceByIdAsync(string id)
        {
            var invoice = await _context.Invoice
                                .Include(p => p.Checklist)
                                .FirstOrDefaultAsync(p => p.Id == id);

            return _invoiceUtilities.InvoiceToResponseDto(invoice);
        }


        public async Task<InvoiceResponseDto> GetInvoiceByChecklistIdAsync(string id)
        {
            var invoice = await _context.Invoice
                                .Include(p => p.Checklist)
                                .FirstOrDefaultAsync(p => p.ChecklistId == id);

            return _invoiceUtilities.InvoiceToResponseDto(invoice);
        }

        public async Task<string> CreateInvoiceAsync(InvoiceCreateDto invoiceDto)
        {

            var invoice = new Invoice
            {
                Status = InvoiceStatus.Unpaid,
                CreatedDate = DateTime.Now,
                ChecklistId = invoiceDto.ChecklistId,
                Receiver = invoiceDto.Receiver,
                ReceiverEmail = invoiceDto.ReceiverEmail,
                Amount = invoiceDto.Amount
            };

            _context.Invoice.Add(invoice);
            await _context.SaveChangesAsync();

            return invoice.Id;
        }

        public async Task UpdateInvoiceAsync(InvoiceUpdateDto updatedInvoice)
        {
            var invoice = await _context.Invoice.FirstOrDefaultAsync(u => u.Id == updatedInvoice.Id);

            if (invoice != null)
            {
                if (updatedInvoice.Receiver != null)
                {
                    invoice.Receiver = updatedInvoice.Receiver;
                }

                if (updatedInvoice.ReceiverEmail != null)
                {
                    invoice.ReceiverEmail = updatedInvoice.ReceiverEmail;
                }

                if (updatedInvoice.Amount != null)
                {
                    invoice.Amount = updatedInvoice.Amount;
                }

                if (updatedInvoice.Status != null)
                {
                    string status = updatedInvoice.Status.ToLower();
                    switch (status)
                    {
                        case "Paid":
                            invoice.Status = InvoiceStatus.Paid;
                            break;
                        case "Unpaid":
                            invoice.Status = InvoiceStatus.Unpaid;
                            break;
                        default:
                            break;
                    }
                }

                invoice.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteInvoiceAsync(string id)
        {
            var invoice = await _context.Invoice.FirstOrDefaultAsync(u => u.Id == id);

            if (invoice != null)
            {
                _context.Invoice.Remove(invoice);
                await _context.SaveChangesAsync();
            }
        }

    }

}