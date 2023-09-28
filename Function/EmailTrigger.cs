using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Azure.Communication.Email;
using Azure;
using System.Net.Mime;
using Azure.Storage.Blobs;
using Azure.Identity;

namespace TurbinSikker.EmailTrigger
{
    public static class EmailTrigger
    {
        
        
        [FunctionName("EmailTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            string recipient = req.Query["recipient"];
            string blobRef = req.Query["blobRef"];

            string containerEndpoint = "https://bsturbinsikkertest.blob.core.windows.net/pdf-container";
            BlobContainerClient containerClient = new BlobContainerClient(
                new Uri(containerEndpoint), 
                new DefaultAzureCredential(
                    new DefaultAzureCredentialOptions { ManagedIdentityClientId = "7ba87be7-bb2b-4f09-b4d0-b47c27191947"}
                ));

            Stream stream = new MemoryStream();
            var blobClient = containerClient.GetBlobClient(blobRef);
            await blobClient.DownloadToAsync(stream);
            stream.Position = 0;

            string connectionString = "endpoint=https://turbinsikker-comms-prod.norway.communication.azure.com/;accesskey=Gd3Q7tfeFLa3zeylVJP/RH7QxvZ2Egp2qmTQCxzMTplLaoFuWgIqgRoaCwvUMhAJrc77rkNljwtJV7ye+fYLFQ==";
            var emailClient = new EmailClient(connectionString);

            var emailContent = new EmailContent("Invoice")
            {
                PlainText = "This is your requested invoice",
                Html = "<html><h1>Here is your requested invoice</h1></html>"
            };

            var emailMessage = new EmailMessage(
                senderAddress: "DoNotReply@74f652f3-add8-4eef-9e3b-f91509bb546d.azurecomm.net",
                recipientAddress: recipient,
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
                throw;
            }

            return new OkObjectResult("Email was sent successfully");
        }
    }
}
