using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs
{
    public class LoginDto
    {
        [Required]
        [MaxLength(200)]
        public string Username { get; set; }

        [Required]
        [MaxLength(200)]
        public string Password { get; set; }
    }
}
