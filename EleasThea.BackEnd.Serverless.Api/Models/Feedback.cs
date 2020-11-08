using System.ComponentModel.DataAnnotations;

namespace EleasThea.BackEnd.Serverless.Api.Models
{
    public class Feedback
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
