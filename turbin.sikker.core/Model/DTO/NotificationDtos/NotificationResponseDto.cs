namespace turbin.sikker.core.Model.DTO
{
    public class NotificationResponseDto
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public NotificationStatus NotificationStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
