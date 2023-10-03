using System.Security.AccessControl;
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace Company.Function
{
    public class InvoiceTrigger
    {
        private readonly ILogger<InvoiceTrigger> _logger;

        private static readonly HttpClient client = new HttpClient();

        public InvoiceTrigger(ILogger<InvoiceTrigger> log)
        {
            _logger = log;
        }

        public class Invoice
        {
            public string Receiver;
            public string Amount;
        }

        [FunctionName("InvoiceTrigger")]
        public async Task Run([ServiceBusTrigger("invoice-added", "send-invoice-backend", Connection = "connectionString")]string mySbMsg)
        {
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");


            var message = JsonSerializer.Deserialize<Invoice>(mySbMsg);

            var values = new Dictionary<string, string>
            {
                { "Amount", message.Amount },
                { "Receiver", message.Receiver }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("https://turbinsikker-api-lin-prod.azurewebsites.net/api/AddInvoice", content);

            var responseString = await response.Content.ReadAsStringAsync();
    }
}
}
