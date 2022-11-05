using MathQuizCreatorAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs
{
    /// <summary>
    /// Use this model as the input value when a user wants to grade a quiz.
    /// Used in the QuizzesLearner controller when doing POST. 
    /// </summary>
    public class AnsweredQuiz : Entity
    {
        [Required]
        public Guid QuizId { get; set; }

        [Required]
        public List<AnsweredQuestion> AnsweredQuestions { get; set; }
    }
}
