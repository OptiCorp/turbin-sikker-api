using System.Text.Json;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
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
        private readonly IChecklistService _checklistService;

        public InvoiceService(TurbinSikkerDbContext context, IInvoiceUtilities invoiceUtilities, IChecklistService checklistService)
        {
            _context = context;
            _invoiceUtilities = invoiceUtilities;
            _checklistService = checklistService;
        }

        public async Task<IEnumerable<InvoiceResponseDto>> GetAllInvoicesAsync()
        {
            return await _context.Invoice
                            .Include(p => p.Workflows)
                            .OrderByDescending(c => c.CreatedDate)
                            .Select(p => _invoiceUtilities.InvoiceToResponseDto(p, null))
                            .ToListAsync();
        }

        public async Task<InvoiceResponseDto> GetInvoiceByIdAsync(string id)
        {
            var invoice = await _context.Invoice
                                .Include(p => p.Workflows)
                                .FirstOrDefaultAsync(p => p.Id == id);
            
            if (invoice == null) return null;

            return _invoiceUtilities.InvoiceToResponseDto(invoice, null);
        }


        public async Task<InvoiceResponseDto> GetInvoicePdfByInvoiceIdAsync(string id)
        {
            var invoice = await _context.Invoice.FirstOrDefaultAsync(i => i.Id == id);
            if (invoice == null) return null;

            string containerEndpoint = "https://bsturbinsikkertest.blob.core.windows.net/pdf-container";

            BlobContainerClient containerClient = new BlobContainerClient(new Uri(containerEndpoint), new DefaultAzureCredential());

            var stream = new MemoryStream();
            var blobClient = containerClient.GetBlobClient(invoice.PdfBlobLink);

            await blobClient.DownloadToAsync(stream);
            stream.Position = 0;

            Stream file = File.Create("test.pdf");
            await stream.CopyToAsync(file);


            var invoiceResponse = _invoiceUtilities.InvoiceToResponseDto(invoice, stream.ToArray());

            return invoiceResponse;
        }


        public async Task CreateInvoiceAsync(InvoiceCreateDto invoiceDto)
        {
            ICollection<WorkflowInfo> workflowInfos = new List<WorkflowInfo>();
            int totalAmount = 0;
            for (int i = 0; i < invoiceDto.WorkflowIds.Count; i++)
            {
                var workflow = await _context.Workflow.FirstOrDefaultAsync(w => w.Id == invoiceDto.WorkflowIds.ElementAt(i));
                var checklist = await _checklistService.GetChecklistByIdAsync(workflow.ChecklistId);
                if (workflow.Status == WorkflowStatus.Done)
                {
                    var workflowInfo = new WorkflowInfo
                    {
                        Id = workflow.Id,
                        Name = checklist.Title,
                        CompletionTime = workflow.CompletionTimeMinutes.Value,
                        HourlyRate = invoiceDto.HourlyRate,
                        EstimatedCompletionTime = checklist.EstCompletionTimeMinutes.Value
                    };
                    totalAmount += workflowInfo.HourlyRate*workflowInfo.CompletionTime;
                    workflowInfos.Add(workflowInfo);
                }
            }

            var invoice = new InvoiceSendDto
            {
                Receiver = invoiceDto.Receiver,
                Amount = totalAmount,
                Workflows = workflowInfos
            };

            var connectionsString = Environment.GetEnvironmentVariable("SbConnectionString");
            var sbClient = new ServiceBusClient(connectionsString);
            var sender = sbClient.CreateSender("generate-invoice");
            var body = JsonSerializer.Serialize(invoice);
            var sbMessage = new ServiceBusMessage(body);
            await sender.SendMessageAsync(sbMessage);
        }

        public async Task UpdateInvoiceAsync(InvoiceUpdateDto updatedInvoice)
        {
            var invoice = await _context.Invoice.FirstOrDefaultAsync(u => u.Id == updatedInvoice.Id);

            if (invoice != null)
            {
                // if (updatedInvoice.Receiver != null)
                // {
                //     invoice.Receiver = updatedInvoice.Receiver;
                // }

                // if (updatedInvoice.ReceiverEmail != null)
                // {
                //     invoice.ReceiverEmail = updatedInvoice.ReceiverEmail;
                // }

                // if (updatedInvoice.Amount != null)
                // {
                //     invoice.Amount = updatedInvoice.Amount;
                // }

                if (updatedInvoice.Status != null)
                {
                    string status = updatedInvoice.Status.ToLower();
                    switch (status)
                    {
                        case "paid":
                            invoice.Status = InvoiceStatus.Paid;
                            break;
                        case "unpaid":
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