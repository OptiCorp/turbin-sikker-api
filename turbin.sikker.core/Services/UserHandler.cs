using System.Text;
using System.Text.Json;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;



namespace turbin.sikker.core.Services
{
    public class UserHandler : BackgroundService
    {
        private readonly AppSettings _appSettings;
        private IQueueClient _orderQueueClient;
        public readonly IServiceProvider _serviceProdiver;

        public UserHandler(IOptions<AppSettings> appSettings, IServiceProvider serviceProvider)
        {
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
            _serviceProdiver = serviceProvider;
        }

        public async Task Handle(Message message, CancellationToken cancelToken)
        {
            using (var scope = _serviceProdiver.CreateScope())
            {
                if (message == null)
                    throw new ArgumentNullException(nameof(message));

                var scopedService = scope.ServiceProvider.GetRequiredService<TurbinSikkerDbContext>();
                var userService = scope.ServiceProvider.GetRequiredService<UserService>();

                var body = Encoding.UTF8.GetString(message.Body);

                Console.WriteLine(body);

                UserBusDto userBody = JsonSerializer.Deserialize<UserBusDto>(body);

                Console.WriteLine(userBody);

                switch (userBody.Action.ToLower())
                {
                    case "created":
                        var userCreatedDto = new UserCreateDto
                        {
                            Username = userBody.Username,
                            AzureAdUserId = userBody.AzureAdUserId,
                            FirstName = userBody.FirstName,
                            LastName = userBody.LastName,
                            Email = userBody.Email,
                            UserRoleId = userBody.UserRole
                        };
                        await userService.CreateUserAsync(userCreatedDto);
                        break;

                    case "updated":
                        var userUpdatedDto = new UserUpdateDto
                        {
                            Id = userBody.Id,
                            Username = userBody.Username,
                            FirstName = userBody.FirstName,
                            LastName = userBody.LastName,
                            Email = userBody.Email,
                            UserRoleId = userBody.UserRole,
                            Status = userBody.Status
                        };
                        await userService.UpdateUserAsync(userUpdatedDto);
                        break;

                    case "softdeleted":
                        await userService.DeleteUserAsync(userBody.Id);
                        break;

                    case "harddeleted":
                        await userService.HardDeleteUserAsync(userBody.Id);
                        break;

                    default:
                        break;
                }

                await scopedService.SaveChangesAsync();

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
            _orderQueueClient = new QueueClient(_appSettings.QueueConnectionString, _appSettings.QueueNameUser);
            _orderQueueClient.RegisterMessageHandler(Handle, messageHandlerOptions);
            Console.WriteLine($"{nameof(UserHandler)} service has started.");
            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine($"{nameof(UserHandler)} service has stopped.");
            await _orderQueueClient.CloseAsync().ConfigureAwait(false);
        }

    }
}
