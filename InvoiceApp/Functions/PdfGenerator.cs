using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Storage.Blobs;
using InvoiceApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace InvoiceApp.Functions
{
    public class PdfGenerator
    {
        private readonly InvoiceContext _context;
        public PdfGenerator(InvoiceContext invoiceContext)
        {
            _context = invoiceContext;
        }

        [Function("PdfGenerator")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestData req)
        {
            var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            var invoiceId = query["invoiceId"];
            var code = query["code"];

            Invoice invoice = await _context.Invoice.FirstOrDefaultAsync(i => i.Id == invoiceId);

            string containerEndpoint = "https://bsturbinsikkertest.blob.core.windows.net/pdf-container";
            BlobContainerClient containerClient = new BlobContainerClient(
                new Uri(containerEndpoint), 
                new DefaultAzureCredential(
                    new DefaultAzureCredentialOptions {ManagedIdentityClientId = Environment.GetEnvironmentVariable("AZURE_CLIENT_ID")}
                ));

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
                new XRect(0, page.Height*0/6, page.Width, page.Height/6),
                XStringFormats.Center);

            gfx.DrawString(string.Format("Date sent: {0}", invoice.SentDate), font, XBrushes.Black,
                new XRect(0, page.Height*1/6, page.Width, page.Height/6),
                XStringFormats.Center);

            gfx.DrawString(string.Format("Hello {0}, this is your invoice", invoice.Reciever), font, XBrushes.Black,
                new XRect(0, page.Height*2/6, page.Width, page.Height/6),
                XStringFormats.Center);

            gfx.DrawString(string.Format("This invoice is from: {0}", invoice.Sender), font, XBrushes.Black,
                new XRect(0, page.Height*3/6, page.Width, page.Height/6),
                XStringFormats.Center);

            gfx.DrawString(string.Format("Invoice status: {0}", invoice.Status), font, XBrushes.Black,
                new XRect(0, page.Height*4/6, page.Width, page.Height/6),
                XStringFormats.Center);

            gfx.DrawString(string.Format("Your total is: {0}kr", invoice.Amount), font, XBrushes.Black,
                new XRect(0, page.Height*5/6, page.Width, page.Height/6),
                XStringFormats.Center);



            try
            {
                await containerClient.CreateIfNotExistsAsync();

                using (MemoryStream blobStream = new MemoryStream())
                {
                    document.Save(blobStream, false);
                    blobStream.Position = 0;
                    await containerClient.UploadBlobAsync(invoice.PdfBlobLink, blobStream);
                }
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }

            var client = new HttpClient();
            

            var response = await client.PostAsync(
                string.Format("https://turbinsikker-fa-prod.azurewebsites.net/api/EmailSender?code={0}&invoiceId={1}", code, invoice.Id),
                null);

            if (response.StatusCode != HttpStatusCode.OK) return new BadRequestObjectResult("Email sender failed");


            return new OkResult();
        }
    }
}