using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs.Parameter
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Parameter Add Dto. Used when receiving a parameter to add.
    /// </summary>
    public class ParameterAddDto
    {
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
