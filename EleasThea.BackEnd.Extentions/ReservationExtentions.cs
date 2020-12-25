using EleasThea.BackEnd.Contracts.Enums;
using EleasThea.BackEnd.Contracts.TableStorageModels;
using System;

namespace EleasThea.BackEnd.Extentions
{
    /// <summary>
    /// Extentions used mainly in reservation related work.
    /// </summary>
    public static class ReservationExtentions
    {
        /// <summary>
        /// Assign reservation type to a reservation instance.
        /// </summary>
        /// <param name="reservation">Reservation instance to assign type.</param>
        /// <param name="reservationTypeAsString">Reservation Type as string.</param>
        /// <returns>Reservation instance with the reservation property set.</returns>
        public static Reservation AssignReservationType(this Reservation reservation, string reservationTypeAsString)
        {
            if (reservationTypeAsString.ToLower() == ReservationType.CookingClass.ToString().ToLower())
                reservation.Type = ReservationType.CookingClass;
            else if (reservationTypeAsString.ToLower() == ReservationType.Table.ToString().ToLower())
                reservation.Type = ReservationType.Table;
            else
                throw new ArgumentException(nameof(reservationTypeAsString), $"'{reservationTypeAsString}' is not a valid Reservation Type. Please use \"CookingClass\" or \"Table\"");

            return reservation;
        }

        /// <summary>
        /// Assign language to a reservation instance.
        /// </summary>
        /// <param name="reservation">Reservation instance to assign type.</param>
        /// <param name="languageAsString">Language as string</param>
        /// <returns>Reservation instance with the language property set.</returns>
        public static Reservation AssignLanguage(this Reservation reservation, string languageAsString)
        {
            if (languageAsString.ToLower() == Language.EN.ToString().ToLower())
                reservation.MadeInLanguage = Language.EN;
            else if (languageAsString.ToLower() == Language.FR.ToString().ToLower())
                reservation.MadeInLanguage = Language.FR;
            else if (languageAsString.ToLower() == Language.IT.ToString().ToLower())
                reservation.MadeInLanguage = Language.IT;
            else if (languageAsString.ToLower() == Language.GR.ToString().ToLower())
                reservation.MadeInLanguage = Language.GR;
            else
                throw new ArgumentException($"{languageAsString} is not a valid language. Please use \"en\", \"fr\", \"it\" or \"gr\"");

            return reservation;
        }
    }
}
