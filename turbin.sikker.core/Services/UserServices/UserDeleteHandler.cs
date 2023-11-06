using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Services
{
    public class UserDeleteHandler : BackgroundService
    {
        private readonly AppSettings _appSettings;
        private ServiceBusClient _client;
        private ServiceBusProcessor _processor;
        public readonly IServiceProvider _serviceProdiver;

        public UserDeleteHandler(IOptions<AppSettings> appSettings, IServiceProvider serviceProvider)
        {
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
            _serviceProdiver = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Initialize the Service Bus client and processor.
            _client = new ServiceBusClient(_appSettings.QueueConnectionString);
            _processor = _client.CreateProcessor(_appSettings.TopicUserDeleted, _appSettings.SubscriptionTurbinsikker, new ServiceBusProcessorOptions());

            // Configure args handler and error handler.
            _processor.ProcessMessageAsync += MessageHandler;
            _processor.ProcessErrorAsync += ErrorHandler;

            // Start processing messages.
            await _processor.StartProcessingAsync(stoppingToken);

            Console.WriteLine($"{nameof(UserDeleteHandler)} service has started.");

            // Wait for a cancellation signal (e.g., Ctrl + C) or another stopping condition.
            await Task.Delay(Timeout.Infinite, stoppingToken);

            Console.WriteLine($"{nameof(UserDeleteHandler)} service has stopped.");

            // Stop processing messages and clean up resources.
            await _processor.StopProcessingAsync();
            await _processor.DisposeAsync();
            await _client.DisposeAsync();
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            using (var scope = _serviceProdiver.CreateScope())
            {
                if (args == null)
                    throw new ArgumentNullException(nameof(args));

                string body = args.Message.Body.ToString();

                UserBusDeleteDto userBody = JsonSerializer.Deserialize<UserBusDeleteDto>(body);

                var scopedService = scope.ServiceProvider.GetRequiredService<TurbinSikkerDbContext>();

                if (userBody.DeleteMode == "Soft")
                {
                    var user = await scopedService.User.FirstOrDefaultAsync(u => u.UmId == userBody.Id);
                    if (user != null)
                    {
                        user.Status = UserStatus.Deleted;
                    }
                }

                if (userBody.DeleteMode == "Hard")
                {
                    var user = await scopedService.User.FirstOrDefaultAsync(u => u.UmId == userBody.Id);
                    if (user != null)
                    {
                        scopedService.User.Remove(user);
                    }
                }

                await scopedService.SaveChangesAsync();
                await args.CompleteMessageAsync(args.Message);
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));
            return Task.CompletedTask;
        }
    }
}
