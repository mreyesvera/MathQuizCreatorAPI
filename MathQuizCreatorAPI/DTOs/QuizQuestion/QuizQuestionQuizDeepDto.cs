using MathQuizCreatorAPI.DTOs.Quiz;
using MathQuizCreatorAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs.QuizQuestion
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Quiz Question Quiz Deep Dto. Holds the quiz question data, but
    /// only holds the quiz's associated data. 
    /// </summary>
    public class QuizQuestionQuizDeepDto
    {
        [Required]
        public Guid QuizQuestionId { get; set; }

        [Required]
        public Guid? QuestionId { get; set; }

        [Required]
        public Guid? QuizId { get; set; }

        [Required]
        public QuizSimplifiedDto? Quiz { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
