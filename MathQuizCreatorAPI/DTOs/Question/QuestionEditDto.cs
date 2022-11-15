using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs.Question
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Question Edit Dto. Used when receiving question data to edit.
    /// </summary>
    public class QuestionEditDto
    {
        [Required]
        public Guid QuestionId { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Answer { get; set; }
    }
}
