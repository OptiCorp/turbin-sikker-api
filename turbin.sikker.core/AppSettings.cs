public class AppSettings
{
    public string QueueConnectionString { get; set; } = "Endpoint=sb://servicebus-turbinsikker-prod.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=jsxc2wM5vV4rhtevLn921gUZCcs7eLEsg+ASbHwJEng=";
    public string QueueName { get; set; } = "add-invoice";
    public string QueueNameNotification { get; set; } = "notification";
    public string QueueNameUser { get; set; } = "user";
}
