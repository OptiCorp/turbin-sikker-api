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
    public class UserUpdateHandler : BackgroundService
    {
        private readonly AppSettings _appSettings;
        private ServiceBusClient _client;
        private ServiceBusProcessor _processor;
        public readonly IServiceProvider _serviceProdiver;

        public UserUpdateHandler(IOptions<AppSettings> appSettings, IServiceProvider serviceProvider)
        {
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
            _serviceProdiver = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Initialize the Service Bus client and processor.
            _client = new ServiceBusClient(_appSettings.QueueConnectionString);
            _processor = _client.CreateProcessor(_appSettings.TopicUserUpdated, _appSettings.SubscriptionTurbinsikker, new ServiceBusProcessorOptions());

            // Configure args handler and error handler.
            _processor.ProcessMessageAsync += MessageHandler;
            _processor.ProcessErrorAsync += ErrorHandler;

            // Start processing messages.
            await _processor.StartProcessingAsync(stoppingToken);

            Console.WriteLine($"{nameof(UserUpdateHandler)} service has started.");

            // Wait for a cancellation signal (e.g., Ctrl + C) or another stopping condition.
            await Task.Delay(Timeout.Infinite, stoppingToken);

            Console.WriteLine($"{nameof(UserUpdateHandler)} service has stopped.");

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
                UserUpdateDto updatedUserDto = JsonSerializer.Deserialize<UserUpdateDto>(body);

                Console.WriteLine(updatedUserDto.Username);

                var scopedService = scope.ServiceProvider.GetRequiredService<TurbinSikkerDbContext>();

                var user = await scopedService.User.FirstOrDefaultAsync(u => u.UmId == updatedUserDto.Id);
                if (user != null)
                {
                    if (updatedUserDto.Username != null)
                        user.Username = updatedUserDto.Username;
                    if (updatedUserDto.FirstName != null)
                        user.FirstName = updatedUserDto.FirstName;
                    if (updatedUserDto.LastName != null)
                        user.LastName = updatedUserDto.LastName;
                    if (updatedUserDto.Email != null)
                        user.Email = updatedUserDto.Email;
                    if (updatedUserDto.UserRole != null)
                        user.UserRole = updatedUserDto.UserRole;
                    if (updatedUserDto.AzureAdUserId != null)
                        user.AzureAdUserId = updatedUserDto.AzureAdUserId;
                    if (updatedUserDto.Status != null)
                    {
                        string status = updatedUserDto.Status.ToLower();
                        switch (status)
                        {
                            case "active":
                                user.Status = UserStatus.Active;
                                break;
                            case "disabled":
                                user.Status = UserStatus.Disabled;
                                break;
                            case "deleted":
                                user.Status = UserStatus.Deleted;
                                break;
                            default:
                                break;
                        }
                    }
                }
                user.UpdatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));
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
