using MathQuizCreatorAPI.DTOs.Parameter;
using MathQuizCreatorAPI.DTOs.QuizQuestion;
using MathQuizCreatorAPI.DTOs.Topic;
using MathQuizCreatorAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs.Question
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Question Deep Dto. Used when sending data holding the respective
    /// values of relationships. Used to avoid errors due to the 
    /// circular reference in JSON. 
    /// </summary>
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

        public List<ParameterSimplifiedDto>? Parameters { get; set; }

    }
}
