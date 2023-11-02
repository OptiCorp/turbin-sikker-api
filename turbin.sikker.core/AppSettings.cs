public class AppSettings
{
    public string QueueConnectionString { get; set; } = "Endpoint=sb://servicebus-turbinsikker-prod.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=jsxc2wM5vV4rhtevLn921gUZCcs7eLEsg+ASbHwJEng=";
    public string QueueName { get; set; } = "add-invoice";
    public string QueueNameNotification { get; set; } = "notification";
    public string QueueNameUser { get; set; } = "user";



    public string TopicUserCreated { get; set; } = "user-created";
    public string TopicUserUpdated { get; set; } = "user-updated";
    public string TopicUserSoftDeleted { get; set; } = "user-soft-deleted";
    public string TopicUserHardDeleted { get; set; } = "user-hard-deleted";

    public string SubscriptionTurbinsikker { get; set; } = "turbinsikker";
    public string SubscriptionInventory { get; set; } = "inventory";
}
