using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs.QuizLearner
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Answered Question Dto. Used when receiving an answered question.
    /// </summary>
    public class AnsweredQuestion
    {
        [Required]
        public Guid QuestionId { get; set; }

        [Required]
        public string Answer { get; set; }
    }
}
