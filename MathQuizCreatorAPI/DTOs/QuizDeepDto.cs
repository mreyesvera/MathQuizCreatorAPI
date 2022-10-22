using MathQuizCreatorAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs
{
    public class QuizDeepDto : Entity
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

        public List<QuizQuestionQuestionDeepDto>? QuizQuestions { get; set; }

        public TopicSimplifiedDto? Topic { get; set; }
    }
}
