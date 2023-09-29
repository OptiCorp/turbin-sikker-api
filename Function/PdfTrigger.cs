using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using System.IO;
using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs;
using Azure.Identity;

//How to send data to this function
// var invoiceJSON = JsonSerializer.Serialize<Invoice>(invoice, new JsonSerializerOptions { WriteIndented = true });
// StringContent stringContent = new StringContent(invoiceJSON);
// using (var client = new HttpClient())
// {
//     var response = await client.PostAsync(
//         "https://turbinsikker-fa-prod.azurewebsites.net/api/PdfTrigger?code=zSsZk25gZaDKeYrIXLA9Tpn-DFDsQ4HUHhJeRrn8g6W4AzFuOyZJzg==",
//         stringContent);
// }


namespace TurbinSikker.PdfTrigger
{
    public class InvoiceDbContext : DbContext
    {
        public InvoiceDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Invoice> Invoice { get; set; }
    }

    public class Invoice
    {
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
        public DateTime UpdatedDate { get; set; }
        // public string Reciever { get; set; }
        // public string RecieverEmail { get; set; }
        // public string Sender { get; set; }
        // public string Status { get; set; }
        // public int Amount { get; set; }
        // public DateTime SentDate { get; set; }
        // public DateTime DueDate { get; set; }
        // public string BlobRef { get; set; }
    }

    public static class PdfTrigger
    {
        [FunctionName("PdfTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            var connString = GetEnvironmentVariable("ManagedIdentityClientId");
            var optionsBuilder = new DbContextOptionsBuilder<InvoiceDbContext>();
            optionsBuilder.UseSqlServer(GetEnvironmentVariable("InvoiceDbConnectionString"));
            InvoiceDbContext _context = new InvoiceDbContext(optionsBuilder.Options);


            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Invoice invoice = JsonSerializer.Deserialize<Invoice>(requestBody);

            await _context.Invoice.AddAsync(invoice);
            await _context.SaveChangesAsync();

            string containerEndpoint = "https://bsturbinsikkertest.blob.core.windows.net/pdf-container";
            BlobContainerClient containerClient = new BlobContainerClient(
                new Uri(containerEndpoint), 
                new DefaultAzureCredential(
                    new DefaultAzureCredentialOptions { ManagedIdentityClientId = GetEnvironmentVariable("ManagedIdentityClientId")}
                ));

            //Create new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Created with PDFsharp";
            

            //Create an empty page
            PdfPage page = document.AddPage();
            

            //Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            //Create a font
            XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);

            //Draw the text
            gfx.DrawString(string.Format("Invoice ID: {0}", invoice.Id), font, XBrushes.Black,
                new XRect(0, page.Height*0/7, page.Width, page.Height/7),
                XStringFormats.Center);

            gfx.DrawString(string.Format("Date sent: {0}", invoice.Id), font, XBrushes.Black,
                new XRect(0, page.Height*1/7, page.Width, page.Height/7),
                XStringFormats.Center);

            gfx.DrawString(string.Format("Hello {0}, this is your invoice", invoice.Id), font, XBrushes.Black,
                new XRect(0, page.Height*2/7, page.Width, page.Height/7),
                XStringFormats.Center);

            gfx.DrawString(string.Format("This invoice is from: {0}", invoice.Id), font, XBrushes.Black,
                new XRect(0, page.Height*3/7, page.Width, page.Height/7),
                XStringFormats.Center);

            gfx.DrawString(string.Format("Invoice status: {0}", invoice.Id), font, XBrushes.Black,
                new XRect(0, page.Height*4/7, page.Width, page.Height/7),
                XStringFormats.Center);

            gfx.DrawString(string.Format("Your total is: {0}kr", invoice.Id), font, XBrushes.Black,
                new XRect(0, page.Height*5/7, page.Width, page.Height/7),
                XStringFormats.Center);

            gfx.DrawString(string.Format("Your payment is due: ", invoice.Id), font, XBrushes.Black,
                new XRect(0, page.Height*6/7, page.Width, page.Height/7),
                XStringFormats.Center);

            // Upload to blob
            try
            {
                await containerClient.CreateIfNotExistsAsync();

                using (MemoryStream blobStream = new MemoryStream())
                {
                    document.Save(blobStream, false);
                    blobStream.Position = 0;
                    await containerClient.UploadBlobAsync(invoice.Id, blobStream);
                }
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }

            return new OkResult();
            
        }

        public static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}
