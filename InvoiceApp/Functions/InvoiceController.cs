using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net.Http;
using System.Net;
using System;
using InvoiceApp.Model;
using System.Text.Json;
using Azure.Messaging.ServiceBus;


namespace InvoiceApp.Functions
{
    public class InvoiceController
    {
        private readonly InvoiceContext _context;
        public InvoiceController(InvoiceContext invoiceContext)
        {
            _context = invoiceContext;
        }

        [Function("InvoiceController")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestData req)
        {
            var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            var code = query["code"];

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            InvoiceDto invoiceDto = JsonSerializer.Deserialize<InvoiceDto>(requestBody);

            Invoice invoice = new Invoice
            {
                CreatedDate = DateTime.Now,
                SentDate = DateTime.Now,
                Status = InvoiceStatus.Unpaid,
                Sender = invoiceDto.Sender,
                Receiver = invoiceDto.Receiver,
                Amount = invoiceDto.Amount,
                PdfBlobLink = Guid.NewGuid().ToString()
            };
            
            await _context.Invoice.AddAsync(invoice);
            await _context.SaveChangesAsync();
        
            var client = new HttpClient();
            var response = await client.PostAsync(
                string.Format("https://turbinsikker-fa-prod.azurewebsites.net/api/PdfGenerator?code={0}&invoiceId={1}", code, invoice.Id),
                null
            );

            if (response.StatusCode != HttpStatusCode.OK) return new BadRequestObjectResult("Pdf-generation failed");

            var connectionString = "Endpoint=sb://servicebus-turbinsikker-prod.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=jsxc2wM5vV4rhtevLn921gUZCcs7eLEsg+ASbHwJEng=";
            var sbClient = new ServiceBusClient(connectionString);
            var sender = sbClient.CreateSender("invoice-added");

            var body = JsonSerializer.Serialize(invoice);

            var sbMessage = new ServiceBusMessage(body);
            await sender.SendMessageAsync(sbMessage);


            return new OkObjectResult("Success");
        }
    }
}