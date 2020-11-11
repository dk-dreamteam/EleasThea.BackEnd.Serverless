using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace EleasThea.BackEnd.Contracts.TableStorageModels
{
    public class CookingClassesReservation : TableEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string NumberOfPersons { get; set; }
        public DateTime DateTimeOfReservation { get; set; }
        public EmailTransmissionStatus TransmissionToRestaurantStatus { get; set; }
        public EmailTransmissionStatus TransmissionToCustomerStatus { get; set; }
    }
}
