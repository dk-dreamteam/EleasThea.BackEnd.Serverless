using EleasThea.BackEnd.Contracts.InputModels;
using EleasThea.BackEnd.Contracts.QueueModels;
using EleasThea.BackEnd.Contracts.TableStorageModels;

namespace EleasThea.BackEnd.Extentions
{
    public static class MappingExtentions
    {
        #region Reservation Mapping
        /// <summary>
        /// Map InputModel to QueueModel.
        /// </summary>
        /// <param name="reservation">Reservation input model to map.</param>
        /// <returns>ReservationQueueItem</returns>
        public static ReservationQueueItem MapToQueueItem(this ReservationMessage reservation) => new ReservationQueueItem
        {
            DateTimeOfReservation = reservation.DateTimeOfReservation,
            Email = reservation.Email,
            FullName = reservation.FullName,
            NumberOfPersons = reservation.NumberOfPersons,
            Tel = reservation.Tel,
        };

        /// <summary>
        /// Map InputModel to TableEntity.
        /// </summary>
        /// <param name="reservation">Reservation input model to map.</param>
        /// <returns>Reservation</returns>
        public static Reservation MapToTableEntity(this ReservationQueueItem reservation) => new Reservation
        {
            DateTimeOfReservation = reservation.DateTimeOfReservation,
            Email = reservation.Email,
            FullName = reservation.FullName,
            NumberOfPersons = reservation.NumberOfPersons,
            Tel = reservation.Tel,
            Type = reservation.Type
        };
        #endregion

        #region Feedback Mapping
        /// <summary>
        /// Map InputModel to QueueModel.
        /// </summary>
        /// <param name="feedback">Feedback input model to map.</param>
        /// <returns>FeedbackQueueItem</returns>
        public static FeedbackQueueItem MapToQueueItem(this FeedbackMessage feedback) => new FeedbackQueueItem
        {
            Email = feedback.Email,
            Tel = feedback.Tel,
            FullName = feedback.FullName,
            Message = feedback.Message
        };

        /// <summary>
        /// Map InputModel to Feedback.
        /// </summary>
        /// <param name="feedback">Feedback input model to map.</param>
        /// <returns>Feedback</returns>
        public static Feedback MapToTableEntity(this FeedbackQueueItem feedback) => new Feedback
        {
            Email = feedback.Email,
            Tel = feedback.Tel,
            FullName = feedback.FullName,
            Message = feedback.Message
        };
        #endregion
    }
}
