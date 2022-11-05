using MathQuizCreatorAPI.DTOs.SolvedQuiz;
using MathQuizCreatorAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs
{
    /// <summary>
    /// Use this entity as the return value for a call to grade a quiz.
    /// It contains the solved quiz as well as the answered questions.
    /// The answered questions are not persisted to the database, so they
    /// are just shown to the user after solving the quiz. 
    /// </summary>
    public class AnsweredQuizGraded : Entity
    {
        [Required]
        public Guid QuizId { get; set; }

        [Required]
        public  SolvedQuizSimplifiedDto SolvedQuiz { get; set; }

        [Required]
        public List<AnsweredQuestionGraded> AnsweredQuestions { get; set; }
    }
}
