using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs
{
    public class QuizQuestionQuestionDeepDto
    {
        [Required]
        public Guid QuizQuestionId { get; set; }

        [Required]
        public Guid? QuestionId { get; set; }

        [Required]
        public QuestionSimplifiedDto? Question { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
