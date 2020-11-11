namespace EleasThea.BackEnd.Contracts.QueueDTOs
{
    public class FeedbackDTO : InputMessageDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Message { get; set; }
    }
}
