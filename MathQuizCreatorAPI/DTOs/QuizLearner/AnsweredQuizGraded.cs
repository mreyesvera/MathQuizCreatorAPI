using MathQuizCreatorAPI.DTOs.SolvedQuiz;
using MathQuizCreatorAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs.QuizLearner
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Answered Quiz Graded Dto. 
    /// Used when sending a graded answered quiz.
    /// </summary>
    public class AnsweredQuizGraded : Entity
    {
        [Required]
        public Guid QuizId { get; set; }

        [Required]
        public SolvedQuizSimplifiedDto SolvedQuiz { get; set; }

        [Required]
        public List<AnsweredQuestionGraded> AnsweredQuestionsGraded { get; set; }
    }
}
