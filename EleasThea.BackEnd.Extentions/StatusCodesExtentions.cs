namespace EleasThea.BackEnd.Extentions
{
    public static class StatusCodesExtentions
    {
        /// <summary>
        /// Checks if Status Code:int is Success Code.
        /// </summary>
        /// <param name="statusCode">The http response code to examine.</param>
        /// <returns>True or false depending on successful or not.</returns>
        public static bool IsSuccessStatusCode(this int statusCode)
        {
            // if code is 2XX div by 100 should equal to 2.
            if (statusCode / 100 == 2)
                return true;
            return false;
        }
    }
}
