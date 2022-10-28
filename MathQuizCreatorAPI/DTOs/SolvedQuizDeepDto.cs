using MathQuizCreatorAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs
{
    public class SolvedQuizDeepDto
    {
        [Required]
        public Guid SolvedQuizId { get; set; }

        [Required]
        public UserSimplifiedDto? User { get; set; }

        [Required]
        public QuizSimplifiedDto? Quiz { get; set; }

        [Required]
        public int CorrectResponses { get; set; }

        [Required]
        public int IncorrectResponses { get; set; }

        [Required]
        public int TotalQuestions { get; set; }

        [Required]
        public double Score { get; set; }
    }
}
