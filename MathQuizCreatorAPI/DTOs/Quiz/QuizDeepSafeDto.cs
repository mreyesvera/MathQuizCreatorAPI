using MathQuizCreatorAPI.DTOs.QuizQuestion;
using MathQuizCreatorAPI.DTOs.Topic;
using MathQuizCreatorAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs.Quiz
{
    public class QuizDeepSafeDto : Entity
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
        public UserSimplifiedDto Creator { get; set; }

        public List<QuizQuestionQuestionDeepSafeDto>? QuizQuestions { get; set; }

        public TopicSimplifiedDto? Topic { get; set; }
    }
}
