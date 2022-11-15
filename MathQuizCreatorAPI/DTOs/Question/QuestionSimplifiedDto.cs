using System.ComponentModel.DataAnnotations;
using MathQuizCreatorAPI.Models;
using Newtonsoft.Json;

namespace MathQuizCreatorAPI.DTOs.Question
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Question Simplfied Dto. Used when sending data without
    /// the need for the values of relationships. Used to avoid errors due to the 
    /// circular reference in JSON. 
    /// </summary>
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
    }
}
