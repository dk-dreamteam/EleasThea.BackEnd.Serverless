using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EleasThea.BackEnd.Serverless.Api.Models
{
    public class ReservationTable
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
