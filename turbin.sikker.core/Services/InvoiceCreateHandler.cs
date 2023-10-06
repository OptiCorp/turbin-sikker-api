
using System;  
using System.Text;
using System.Text.Json;
using System.Threading;  
using System.Threading.Tasks;  
using Microsoft.Azure.ServiceBus;  
using Microsoft.Extensions.Hosting;  
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using turbin.sikker.core.Model;
    

      
namespace turbin.sikker.core.Services 
{  
    public class CreateInvoiceHandler : BackgroundService  
    {  
        private readonly AppSettings _appSettings;  
        private IQueueClient _orderQueueClient;  
        public readonly IServiceProvider _serviceProdiver;
    
        public CreateInvoiceHandler(IOptions<AppSettings> appSettings, IServiceProvider serviceProvider)  
        {  
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings)); 
            _serviceProdiver = serviceProvider;
        }  
    
        public async Task Handle(Message message, CancellationToken cancelToken)  
        {  
            using (var scope = _serviceProdiver.CreateScope())
            {
            if (message == null)  
                throw new ArgumentNullException(nameof(message));  
    
            var body = Encoding.UTF8.GetString(message.Body);  
            Console.WriteLine($"Message: {body}");  

            Invoice invoiceBody = JsonConvert.DeserializeObject<Invoice>(body);

                Console.WriteLine($"amount: {invoiceBody.Amount}");  

            Invoice invoice = new Invoice{
                Sender = invoiceBody.Sender,
                Receiver = invoiceBody.Receiver,
                CreatedDate = invoiceBody.CreatedDate,
                SentDate = invoiceBody.SentDate,
                Amount = invoiceBody.Amount,
                PdfBlobLink = invoiceBody.PdfBlobLink,
                Status = invoiceBody.Status
            };
            
            Console.WriteLine($"Invoice: {invoiceBody}");  
            Console.WriteLine($"pdf: {invoice.Sender}");

            var scopedService = scope.ServiceProvider.GetRequiredService<TurbinSikkerDbContext>();

            scopedService.Invoice.AddAsync(invoiceBody);
            scopedService.SaveChangesAsync();

            await _orderQueueClient.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);  
            }
        }  
        public virtual Task HandleFailureMessage(ExceptionReceivedEventArgs exceptionReceivedEventArgs)  
        {  
            if (exceptionReceivedEventArgs == null)  
                throw new ArgumentNullException(nameof(exceptionReceivedEventArgs));  
            return Task.CompletedTask;  
        }  
    
        protected override Task ExecuteAsync(CancellationToken stoppingToken)  
        {  
            var messageHandlerOptions = new MessageHandlerOptions(HandleFailureMessage)  
            {  
                MaxConcurrentCalls = 5,  
                AutoComplete = false,  
                MaxAutoRenewDuration = TimeSpan.FromMinutes(10)  
            };  
            _orderQueueClient = new QueueClient(_appSettings.QueueConnectionString, _appSettings.QueueName);  
            _orderQueueClient.RegisterMessageHandler(Handle, messageHandlerOptions);  
            Console.WriteLine($"{nameof(CreateInvoiceHandler)} service has started.");  
            return Task.CompletedTask;  
        }  
    
        public override async Task StopAsync(CancellationToken stoppingToken)  
        {  
            Console.WriteLine($"{nameof(CreateInvoiceHandler)} service has stopped.");  
            await _orderQueueClient.CloseAsync().ConfigureAwait(false);  
        }  
    
    }  
}  
