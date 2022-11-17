using MathQuizCreatorAPI.DTOs.Parameter;
using MathQuizCreatorAPI.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs.Question
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Question Simplfied Safe Dto. Same as QuestionSimplifiedDto, 
    /// but without the question's answer. 
    /// </summary>
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

        public List<ParameterSimplifiedSafeDto> Parameters { get; set; }
    }
}
