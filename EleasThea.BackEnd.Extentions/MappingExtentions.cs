using EleasThea.BackEnd.Contracts.InputModels;
using EleasThea.BackEnd.Contracts.TableStorageModels;

namespace EleasThea.BackEnd.Extentions
{
    public static class MappingExtentions
    {
        /// <summary>
        /// Map FeedbackMessage to Feedback Table Entity model.
        /// </summary>
        /// <param name="feedbackMessage">FeedbackMessage object to get the properties' values.</param>
        /// <returns>Feedback object with values in </returns>
        public static Feedback MapToTableEntity(this FeedbackMessage feedbackMessage)
        {
            return new Feedback
            {
                Email = feedbackMessage.Email,
                FullName = feedbackMessage.FullName,
                Message = feedbackMessage.Message,
                Tel = feedbackMessage.Tel
            };
        }
    }
}
