namespace EleasThea.BackEnd.Contracts.QueueModels
{
    /// <summary>
    /// Used to transfer feedback items via queues.
    /// </summary>
    public class FeedbackQueueItem
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Message { get; set; }
    }
}
