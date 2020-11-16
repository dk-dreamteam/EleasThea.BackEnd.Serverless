namespace EleasThea.BackEnd.Contracts.QueueModels
{
    public class SendEmailQueueItem
    {
        public string ToEmailAddress { get; set; }
        public string Subject { get; set; }
        public string HtmlContent { get; set; }

    }
}
