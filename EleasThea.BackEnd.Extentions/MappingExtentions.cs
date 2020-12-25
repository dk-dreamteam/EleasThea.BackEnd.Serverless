using EleasThea.BackEnd.Contracts.InputModels;
using EleasThea.BackEnd.Contracts.QueueModels;
using EleasThea.BackEnd.Contracts.TableStorageModels;

namespace EleasThea.BackEnd.Extentions
{
    public static class MappingExtentions
    {
        #region Reservation Mapping
        /// <summary>
        /// Map QueueModel to TableEntity.
        /// </summary>
        /// <param name="reservation">Reservation queue model to map.</param>
        /// <returns>Reservation</returns>
        public static Reservation MapToTableEntity(this ReservationMessage reservation) => new Reservation
        {
            DateTimeOfReservation = reservation.DateTimeOfReservation,
            Email = reservation.Email,
            FullName = reservation.FullName,
            NumberOfPersons = reservation.NumberOfPersons,
            Tel = reservation.Tel,
        };
        #endregion

        #region Feedback Mapping
        /// <summary>
        /// Map InputModel to Feedback.
        /// </summary>
        /// <param name="feedback">Feedback input model to map.</param>
        /// <returns>Feedback</returns>
        public static Feedback MapToTableEntity(this FeedbackMessage feedback) => new Feedback
        {
            Email = feedback.Email,
            Tel = feedback.Tel,
            FullName = feedback.FullName,
            Message = feedback.Message
        };
        #endregion
    }
}
