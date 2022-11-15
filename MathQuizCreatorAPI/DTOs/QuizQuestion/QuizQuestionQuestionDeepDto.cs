using System.ComponentModel.DataAnnotations;
using MathQuizCreatorAPI.DTOs.Question;

namespace MathQuizCreatorAPI.DTOs.QuizQuestion
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Quiz Question Question Deep Dto. Holds the quiz question data, but
    /// only holds the question's associated data. 
    /// </summary>
    public class QuizQuestionQuestionDeepDto
    {
        [Required]
        public Guid QuizQuestionId { get; set; }

        [Required]
        public Guid? QuizId { get; set; }

        [Required]
        public Guid? QuestionId { get; set; }

        [Required]
        public QuestionSimplifiedDto? Question { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
