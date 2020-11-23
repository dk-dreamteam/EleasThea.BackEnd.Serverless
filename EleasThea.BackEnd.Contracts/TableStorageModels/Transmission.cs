using EleasThea.BackEnd.Contracts.Enums;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace EleasThea.BackEnd.Contracts.TableStorageModels
{
    public class Transmission : TableEntity
    {
        //todo: ReservationId
        public TransmissionStatus Status { get; set; }
        public bool Opened { get; set; }
        public DateTime? LastOpenedOnUtc { get; set; }
    }
}
