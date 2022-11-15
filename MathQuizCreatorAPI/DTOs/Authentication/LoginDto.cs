using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs.Authentication
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Login Dto. Used to receive login data from users.  
    /// </summary>
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(200)]
        public string Email { get; set; }

        [Required]
        [MaxLength(200)]
        public string Password { get; set; }
    }
}
