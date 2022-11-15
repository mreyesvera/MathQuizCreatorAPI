using MathQuizCreatorAPI.Models;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using MathQuizCreatorAPI.DTOs.Question;
using MathQuizCreatorAPI.DTOs.Quiz;

namespace MathQuizCreatorAPI.DTOs.Topic
{
    [JsonObject]
    public class TopicDeepDto : Entity
    {
        [Required]
        public Guid TopicId { get; set; }

        [Required]
        public string Title { get; set; }


        public List<QuizSimplifiedDto>? Quizzes { get; set; }

        public List<QuestionSimplifiedDto>? Questions { get; set; }
    }
}
