using System.Text;
using System.Text.Json;
using Azure.Messaging.WebPubSub;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;



namespace turbin.sikker.core.Services
{
    public class NotificationHandler : BackgroundService
    {
        private readonly AppSettings _appSettings;
        private IQueueClient _orderQueueClient;
        public readonly IServiceProvider _serviceProdiver;

        public NotificationHandler(IOptions<AppSettings> appSettings, IServiceProvider serviceProvider)
        {
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
            _serviceProdiver = serviceProvider;
        }

        public async Task Handle(Message message, CancellationToken cancelToken)
        {
            var messageBody = Encoding.UTF8.GetString(message.Body);

            NotificationBusDto notificationBus = JsonSerializer.Deserialize<NotificationBusDto>(messageBody);

            using (var scope = _serviceProdiver.CreateScope())
            {
                if (message == null)
                    throw new ArgumentNullException(nameof(message));

                Notification notification = new Notification
                {
                    Message = notificationBus.Message,
                    NotificationStatus = NotificationStatus.Unread, // TODO: change to read and unread
                    CreatedDate = DateTime.Now,
                    NotificationType = NotificationType.Error,
                    ReceiverId = notificationBus.ReceiverId,
                };

                var scopedService = scope.ServiceProvider.GetRequiredService<TurbinSikkerDbContext>();

                await scopedService.Notification.AddAsync(notification);
                await scopedService.SaveChangesAsync();

                var serviceClient = new WebPubSubServiceClient("Endpoint=https://pub-sub-test.webpubsub.azure.com;AccessKey=QZWtGVoO7OFHum7s53t5ZiukRagDDbtHR9s+DP7WvkI=;Version=1.0;", "hub");
                await serviceClient.SendToAllAsync(notificationBus.Message);

                await _orderQueueClient.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
            }
        }
        public virtual Task HandleFailureMessage(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            if (exceptionReceivedEventArgs == null)
                throw new ArgumentNullException(nameof(exceptionReceivedEventArgs));
            return Task.CompletedTask;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var messageHandlerOptions = new MessageHandlerOptions(HandleFailureMessage)
            {
                MaxConcurrentCalls = 5,
                AutoComplete = false,
                MaxAutoRenewDuration = TimeSpan.FromMinutes(10)
            };
            _orderQueueClient = new QueueClient(_appSettings.QueueConnectionString, _appSettings.QueueNameNotification);
            _orderQueueClient.RegisterMessageHandler(Handle, messageHandlerOptions);
            Console.WriteLine($"{nameof(NotificationHandler)} service has started.");
            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine($"{nameof(NotificationHandler)} service has stopped.");
            await _orderQueueClient.CloseAsync().ConfigureAwait(false);
        }
    }
}
