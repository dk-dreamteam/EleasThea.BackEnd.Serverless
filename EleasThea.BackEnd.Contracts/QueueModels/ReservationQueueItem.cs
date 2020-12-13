using EleasThea.BackEnd.Contracts.Enums;
using System;

namespace EleasThea.BackEnd.Contracts.QueueModels
{
    /// <summary>
    /// Used to transfer reservation items via queues.
    /// </summary>
    public class ReservationQueueItem
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string NumberOfPersons { get; set; }
        public DateTime DateTimeOfReservation { get; set; }
        public ReservationType Type { get; set; }
        public Language MadeInLanguage { get; set; }
    }
}
