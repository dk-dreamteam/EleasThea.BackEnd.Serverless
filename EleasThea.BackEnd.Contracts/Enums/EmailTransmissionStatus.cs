namespace EleasThea.BackEnd.Contracts.Enums
{
    /// <summary>
    /// Used to describe the status of an email transmission.
    /// </summary>
    public enum TransmissionStatus
    {
        // is the default value in initializing parent object.
        Pending,
        Unsuccessful,
        Sent,
    }
}
