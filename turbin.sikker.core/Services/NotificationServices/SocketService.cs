using Azure.Messaging.WebPubSub;

namespace turbin.sikker.core.Services
{
    public class SocketService : ISocketService
    {
        public async Task SendMessage(string notification)
        {
            var connectionString = Environment.GetEnvironmentVariable("pubSubConnectionString");
            var hub = "Hub";

            var serviceClient = new WebPubSubServiceClient(connectionString, hub);
            await serviceClient.SendToAllAsync(notification);
        }
    }
}