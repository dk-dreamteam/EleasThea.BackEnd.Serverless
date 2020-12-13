using EleasThea.BackEnd.Contracts.Enums;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace EleasThea.BackEnd.Contracts.TableStorageModels
{
    public class Transmission : TableEntity
    {
        public string ReferenceToReservationRowKey { get; set; }
        public string ReferenceToFeedbackRowKey { get; set; }
        public TransmissionStatus Status { get; set; }
        public string Body { get; set; }
        public bool Opened { get; set; }
        public DateTime? TransmittedOnUtc { get; set; }
    }
}
