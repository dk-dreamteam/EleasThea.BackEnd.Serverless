using System.ComponentModel.DataAnnotations;

namespace EleasThea.BackEnd.Contracts.InputModels
{
    /// <summary>
    /// Used as incoming binding model for HTTP Triggered Functions.
    /// </summary>
    public class SendFeedback
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Tel { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
