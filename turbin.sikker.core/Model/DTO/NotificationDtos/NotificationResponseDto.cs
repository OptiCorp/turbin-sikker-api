namespace turbin.sikker.core.Model.DTO
{
    public class NotificationResponseDto
    {   
        public string Id { get; set; }
        public string Title { get; set; }
        public string Receiver { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime SentDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
