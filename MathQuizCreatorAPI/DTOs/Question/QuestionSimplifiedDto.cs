using System.ComponentModel.DataAnnotations;
using MathQuizCreatorAPI.Models;
using Newtonsoft.Json;

namespace MathQuizCreatorAPI.DTOs.Question
{
    [JsonObject]
    public class QuestionSimplifiedDto : Entity
    {
        [Required]
        public Guid QuestionId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string? Answer { get; set; }

        public List<string> AssignedQuizzes { get; set; }

        //public QuestionSimplifiedDto(Guid questionId, string title, string description)
        //:super{
        //    QuestionId = questionId;
        //    Title = title;
        //    Description = description;
        //}

    }
}
