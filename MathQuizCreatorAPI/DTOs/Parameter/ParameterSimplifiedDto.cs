using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs.Parameter
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Parameter Dto. Used to transmit parameter data.
    /// </summary>
    public class ParameterSimplifiedDto
    {
        [Required]
        public Guid ParameterId { get; set; }

        [MaxLength(200)]
        [Required]
        public string? Name { get; set; }

        [MaxLength(200)]
        [Required]
        public string? Value { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public Guid? QuestionId { get; set; }

    }
}
