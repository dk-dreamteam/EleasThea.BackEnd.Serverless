using Microsoft.WindowsAzure.Storage.Table;

namespace EleasThea.BackEnd.Contracts.QueueModels
{
    public class SendEmailQueueItem
    {
        public string Subject { get; set; }
        public string HtmlContent { get; set; }
        public TableEntity Entity { get; set; }
    }
}
