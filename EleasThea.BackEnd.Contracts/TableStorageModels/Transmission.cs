using Microsoft.WindowsAzure.Storage.Table;

namespace EleasThea.BackEnd.Contracts.TableStorageModels
{
    public class Transmission : TableEntity
    {
        public TransmissionStatus Status { get; set; }
        public bool Opened { get; set; }
    }
}
