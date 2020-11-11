using System;
using System.ComponentModel.DataAnnotations;

namespace EleasThea.BackEnd.Contracts.InputModels
{
    /// <summary>
    /// Model can be used in both table reservation and cooking lessons reservation.
    /// Used as incoming binding model for HTTP Triggered Functions.
    /// </summary>
    public class Reservation
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Tel { get; set; }

        [Required]
        public string NumberOfPersons { get; set; }

        [Required]
        public DateTime DateTimeOfReservation { get; set; }
    }
}
