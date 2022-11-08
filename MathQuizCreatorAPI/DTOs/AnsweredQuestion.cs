using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs
{
    public class AnsweredQuestion
    {
        [Required]
        public Guid QuestionId { get; set; }

        [Required]
        public string Answer { get; set; }
    }
}
