using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs.Question
{
    public class QuestionEditDto
    {
        [Required]
        public Guid QuestionId { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Answer { get; set; }
    }
}
