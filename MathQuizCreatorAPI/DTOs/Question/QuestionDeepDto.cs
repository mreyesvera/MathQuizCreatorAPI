using MathQuizCreatorAPI.DTOs.QuizQuestion;
using MathQuizCreatorAPI.DTOs.Topic;
using MathQuizCreatorAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs.Question
{
    public class QuestionDeepDto : Entity
    {
        [Required]
        public Guid QuestionId { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Answer { get; set; }

        [Required]
        public TopicSimplifiedDto? Topic { get; set; }

        public List<QuizQuestionQuizDeepDto>? QuizQuestions { get; set; }

    }
}
