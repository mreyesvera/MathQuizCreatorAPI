using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs.Parameter
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Parameter Simplfied Safe Dto. Same as the Parameter Simplified, 
    /// but without disclosing sensitive data. 
    /// </summary>
    public class ParameterSimplifiedSafeDto
    {
        [Required]
        public Guid ParameterId { get; set; }

        //[MaxLength(200)]
        //[Required]
        //public string? Name { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
