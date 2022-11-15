using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs.Authentication
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Register Dto. Used to receive register data from users.  
    /// </summary>
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
        [MinLength(10)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
