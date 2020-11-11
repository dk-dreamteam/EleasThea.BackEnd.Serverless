namespace EleasThea.BackEnd.Contracts
{
    /// <summary>
    /// Used to describe the status email transmission for every entry.
    /// </summary>
    public enum EmailTransmissionStatus
    {
        Pending,
        Unsuccessful,
        Sent,
        Read
    }
}
