using System;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using Azure;
using Azure.Communication.Email;
using Azure.Identity;
using Azure.Storage.Blobs;
using InvoiceApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Functions
{
    public class EmailSender
    {

        private readonly InvoiceContext _context;
        public EmailSender(InvoiceContext invoiceContext)
        {
            _context = invoiceContext;
        }


        [Function("EmailSender")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestData req)
        {
            var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            var invoiceId = query["invoiceId"];

            var invoice = await _context.Invoice.FirstOrDefaultAsync(i => i.Id == invoiceId);

            string containerEndpoint = Environment.GetEnvironmentVariable("PdfContainerEndpoint");
            BlobContainerClient containerClient = new BlobContainerClient(
                new Uri(containerEndpoint), 
                new DefaultAzureCredential(
                    new DefaultAzureCredentialOptions {ManagedIdentityClientId = Environment.GetEnvironmentVariable("AZURE_CLIENT_ID")}
                ));

            Stream stream = new MemoryStream();
            var blobClient = containerClient.GetBlobClient(invoice.PdfBlobLink);
            await blobClient.DownloadToAsync(stream);
            stream.Position = 0;

            string connectionString = Environment.GetEnvironmentVariable("EmailConnectionString");
            var emailClient = new EmailClient(connectionString);

            var emailContent = new EmailContent("Invoice")
            {
                PlainText = "This is your requested invoice",
                Html = "<html><h1>Here is your requested invoice</h1></html>"
            };

            var emailMessage = new EmailMessage(
                senderAddress: "DoNotReply@74f652f3-add8-4eef-9e3b-f91509bb546d.azurecomm.net",
                recipientAddress: invoice.Receiver,
                content: emailContent
            );

            var emailAttachment = new EmailAttachment("Invoice.pdf", MediaTypeNames.Application.Pdf, await BinaryData.FromStreamAsync(stream));
        
            emailMessage.Attachments.Add(emailAttachment);

            try
            {
                await emailClient.SendAsync(WaitUntil.Completed, emailMessage);
            }
            catch (Exception)
            {
                return new BadRequestObjectResult("Email was not sent");
            }

            return new OkObjectResult("Email was sent successfully");
        }
    }
}