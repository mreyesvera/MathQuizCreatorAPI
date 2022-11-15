using MathQuizCreatorAPI.Models;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace MathQuizCreatorAPI.DTOs.Quiz
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Quiz Simplfied Dto. Used when sending data without
    /// the need for the values of relationships. Used to avoid errors due to the 
    /// circular reference in JSON. 
    /// </summary>
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
