using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs.Authentication
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Role Simplified Dto. Used to transmit role data.
    /// </summary>
    public class RoleSimplifiedDto
    {
        [Required]
        public Guid RoleId { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
