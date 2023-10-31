namespace turbin.sikker.core.Model.DTO
{
    public class NotificationResponseDto
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string NotificationStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string NotificationType { get; set; }
        public string ReceiverId { get; set; }
    }
}
