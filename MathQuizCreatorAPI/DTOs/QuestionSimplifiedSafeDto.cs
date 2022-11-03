using MathQuizCreatorAPI.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs
{
    [JsonObject]
    public class QuestionSimplifiedSafeDto : Entity
    {
        [Required]
        public Guid QuestionId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public List<string> AssignedQuizzes { get; set; }
    }
}
