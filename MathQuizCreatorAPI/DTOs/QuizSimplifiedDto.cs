using MathQuizCreatorAPI.Models;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace MathQuizCreatorAPI.DTOs
{
    [JsonObject]
    public class QuizSimplifiedDto : Entity
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

        //public QuizSimplifiedDto(Guid quizId, bool isPublic, bool hasUnlimitedMode, string title, string description)
        //:super{
        //    QuizId = quizId;
        //    IsPublic = isPublic;
        //    HasUnlimitedMode = hasUnlimitedMode;
        //    Title = title;
        //    Description = description;
        //}
    }
}
