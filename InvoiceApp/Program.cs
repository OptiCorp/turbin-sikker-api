using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using InvoiceApp.Model;

namespace InvoiceApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string sqlConnectionString = Environment.GetEnvironmentVariable("InvoiceDbConnectionString");

            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(s => {
                    s.AddDbContext<InvoiceContext>(
                        options => options.UseSqlServer(sqlConnectionString)
                    );
                })
                .Build();

            await host.RunAsync();
        }
    }
}