using System;

namespace EleasThea.BackEnd.Contracts.QueueDTOs
{
    public class ReservationDTO : InputMessageDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string NumberOfPersons { get; set; }
        public DateTime DateTimeOfReservation { get; set; }
    }
}
