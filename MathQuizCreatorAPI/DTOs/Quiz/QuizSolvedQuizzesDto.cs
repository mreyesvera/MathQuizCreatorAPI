using MathQuizCreatorAPI.DTOs.SolvedQuiz;
using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs.Quiz
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Quiz Solved Quizzes Dto. Used to send quiz data along with
    /// its associated solved quizzes.
    /// </summary>
    public class QuizSolvedQuizzesDto
    {
        [Required]
        public Guid QuizId { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        [Required]
        public bool HasUnlimitedMode { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public List<SolvedQuizSimplifiedDto> SolvedQuizzes { get; set; }
    }
}
