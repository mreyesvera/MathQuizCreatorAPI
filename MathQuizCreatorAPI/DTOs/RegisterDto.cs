using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(200)]
        public string Username { get; set; }

        [EmailAddress]
        [MaxLength(200)]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
