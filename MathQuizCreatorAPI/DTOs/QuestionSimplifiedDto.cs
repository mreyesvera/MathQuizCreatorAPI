using System.ComponentModel.DataAnnotations;
using MathQuizCreatorAPI.Models;
using Newtonsoft.Json;

namespace MathQuizCreatorAPI.DTOs
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

        public string AssignedQuizzes { get; set; }

        //public QuestionSimplifiedDto(Guid questionId, string title, string description)
        //:super{
        //    QuestionId = questionId;
        //    Title = title;
        //    Description = description;
        //}

    }
}
