﻿using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace EleasThea.BackEnd.Contracts.TableStorageModels
{
    public class TableReservation : TableEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string NumberOfPersons { get; set; }
        public DateTime DateTimeOfReservation { get; set; }
        public TransmissionStatus TransmissionToRestaurantStatus { get; set; }
        public TransmissionStatus TransmissionToCustomerStatus { get; set; }
    }
}
