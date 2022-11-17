using MathQuizCreatorAPI.DTOs.Parameter;
using MathQuizCreatorAPI.DTOs.QuizQuestion;
using MathQuizCreatorAPI.DTOs.Topic;
using MathQuizCreatorAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs.Question
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Question Deep Parametrized Dto. Same as Question Deep Dto, but the question
    /// description and answer have been parametrized by the parameters held in
    /// the parameters property.
    /// </summary>
    public class QuestionDeepParametrizedDto : Entity
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

        public List<ParameterSimplifiedSafeDto>? Parameters { get; set; }
    }
}
