using EleasThea.BackEnd.Serverless.Services.Models;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace EleasThea.BackEnd.Contracts.TableStorageModels
{
    public class Reservation : TableEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string NumberOfPersons { get; set; }
        public DateTime DateTimeOfReservation { get; set; }
        public ReservationType Type { get; set; }
        public bool? Approved { get; set; }
        public DateTime? ApprovedOnUtc { get; set; }
    }
}
