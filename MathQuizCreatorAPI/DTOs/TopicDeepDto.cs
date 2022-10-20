using MathQuizCreatorAPI.Models;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace MathQuizCreatorAPI.DTOs
{
    [JsonObject]
    public class TopicDeepDto : Entity
    {
        public Guid TopicId { get; set; }

        [Required]
        public string Title { get; set; }


        public List<QuizSimplifiedDto>? Quizzes { get; set; }

        public List<QuestionSimplifiedDto>? Questions { get; set; }
    }
}
