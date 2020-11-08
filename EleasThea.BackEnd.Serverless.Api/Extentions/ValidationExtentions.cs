using System.ComponentModel.DataAnnotations;

namespace EleasThea.BackEnd.Serverless.Api.Extentions
{
    public static class ValidationExtentions
    {
        /// <summary>
        /// Checks if the requested instance is valid based on the model attributes.
        /// </summary>
        /// <param name="object">The instance to check for validity.</param>
        /// <returns>Successful or not of validation.</returns>
        public static bool IsValid(this object @object)
        {
            return Validator.TryValidateObject(@object, new ValidationContext(@object), null);
        }
    }
}
