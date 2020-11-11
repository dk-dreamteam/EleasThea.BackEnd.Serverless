namespace EleasThea.BackEnd.Contracts.QueueDTOs
{
    public class InputMessageWrapperDTO
    {
        public MessageType Type { get; set; }
        public object Message { get; set; }
    }
}
