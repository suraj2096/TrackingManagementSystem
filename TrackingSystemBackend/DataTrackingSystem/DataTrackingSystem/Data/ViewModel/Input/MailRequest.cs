namespace DataTrackingSystem.Data.ViewModel.Input
{
    public class MailRequest
    {
       public string? FromEmail { get; set; }
        public string? ToEmail { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? receiverUserName { get; set; }
        public string? senderUserName { get; set; }
        public string? ReceiverId { get; set; }
    }
}
