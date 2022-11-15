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
    /// Quiz Question Question Deep Safe Dto. Same as QuizQuestionQuestionDeepDto
    /// except that it uses the QuestionSimplifiedSafeDto. 
    /// </summary>
    public class QuizQuestionQuestionDeepSafeDto
    {
        [Required]
        public Guid QuizQuestionId { get; set; }

        [Required]
        public Guid? QuizId { get; set; }

        [Required]
        public Guid? QuestionId { get; set; }

        [Required]
        public QuestionSimplifiedSafeDto? Question { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
