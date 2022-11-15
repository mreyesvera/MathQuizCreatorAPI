using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs.QuizLearner
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Answered Question Graded Dto. 
    /// Used when sending a graded answered question.
    /// </summary>
    public class AnsweredQuestionGraded
    {
        [Required]
        public Guid QuestionId { get; set; }

        [Required]
        public string Answer { get; set; }

        [Required]
        public bool Correct { get; set; }

        [Required]
        public string CorrectAnswer { get; set; }
    }
}
