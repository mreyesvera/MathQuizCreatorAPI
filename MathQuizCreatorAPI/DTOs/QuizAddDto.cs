using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs
{
    public class QuizAddDto
    {
        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        [Required]
        public bool HasUnlimitedMode { get; set; }

        [Required]
        public Guid TopicId { get; set; }

        [Required]
        public Guid CreatorId { get; set; }
    }
}
