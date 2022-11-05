using System.ComponentModel.DataAnnotations;
using MathQuizCreatorAPI.DTOs.Question;

namespace MathQuizCreatorAPI.DTOs.QuizQuestion
{
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
