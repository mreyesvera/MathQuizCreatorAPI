using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs
{
    public class AnsweredQuestionGraded
    {
        [Required]
        public Guid QuestionId { get; set; }

        [Required]
        public string UserAnswer { get; set; }

        [Required]
        public bool Correct { get; set; }

        [Required]
        public string CorrectAnswer { get; set; }
    }
}
