using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Services
{
    public class UserCreateHandler : BackgroundService
    {
        private readonly AppSettings _appSettings;
        private ServiceBusClient _client;
        private ServiceBusProcessor _processor;
        public readonly IServiceProvider _serviceProdiver;

        public UserCreateHandler(IOptions<AppSettings> appSettings, IServiceProvider serviceProvider)
        {
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
            _serviceProdiver = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Initialize the Service Bus client and processor.
            _client = new ServiceBusClient(_appSettings.QueueConnectionString);
            _processor = _client.CreateProcessor(_appSettings.TopicUserCreated, _appSettings.SubscriptionTurbinsikker, new ServiceBusProcessorOptions());

            // Configure args handler and error handler.
            _processor.ProcessMessageAsync += MessageHandler;
            _processor.ProcessErrorAsync += ErrorHandler;

            // Start processing messages.
            await _processor.StartProcessingAsync(stoppingToken);

            Console.WriteLine($"{nameof(UserCreateHandler)} service has started.");

            // Wait for a cancellation signal (e.g., Ctrl + C) or another stopping condition.
            await Task.Delay(Timeout.Infinite, stoppingToken);

            Console.WriteLine($"{nameof(UserCreateHandler)} service has stopped.");

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
                UserBusCreateDto userBody = JsonSerializer.Deserialize<UserBusCreateDto>(body);

                var scopedService = scope.ServiceProvider.GetRequiredService<TurbinSikkerDbContext>();

                var user = new User
                {
                    UmId = userBody.Id,
                    AzureAdUserId = userBody.AzureAdUserId,
                    Username = userBody.Username,
                    FirstName = userBody.FirstName,
                    LastName = userBody.LastName,
                    Email = userBody.Email,
                    UserRoleId = userBody.UserRole,
                    CreatedDate = userBody.CreatedDate,
                    Status = Enum.Parse<UserStatus>(userBody.Status)
                };
                await scopedService.User.AddAsync(user);

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
