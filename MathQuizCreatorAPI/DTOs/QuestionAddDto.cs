using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs
{
    public class QuestionAddDto
    {
        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Answer { get; set; }

        [Required]
        public Guid TopicId { get; set; }

        [Required]
        public Guid CreatorId { get; set; }
    }
}
