using MathQuizCreatorAPI.DTOs.Quiz;
using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs.Topic
{
    public class TopicSolvedQuizzesDto
    {
        [Required]
        public Guid TopicId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public List<QuizSolvedQuizzesDto> Quizzes { get; set; }
    }
}
