using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MathQuizCreatorAPI.Models
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// This model represents a Quiz Question. It holds the many to many
    /// relationship between Quizzes and Questions. It also specifies
    /// an additional field for Order, so the questions show up
    /// in order when displayed to the user. 
    /// </summary>
    [JsonObject]
    public class QuizQuestion
    {
        [Key]
        public Guid QuizQuestionId { get; set; }



        [Required]
        public Guid? QuizId { get; set; }

        private Quiz? _quiz;

        [BackingField(nameof(_quiz))]
        public Quiz? Quiz
        {
            get => _quiz;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Quiz can't be null");
                }

                _quiz = value;
            }
        }

        [Required]
        public Guid? QuestionId { get; set; }

        private Question? _question;

        [BackingField(nameof(_question))]
        public Question? Question
        {
            get => _question;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Question can't be null");
                }

                _question = value;
            }
        }



        [Required]
        public int Order { get; set; }


        public QuizQuestion()
        {

        }

        public QuizQuestion(Quiz quiz, Question question, int order)
        {
            QuizQuestionId = Guid.NewGuid();
            Quiz = quiz;
            QuizId = quiz.QuizId;
            Question = question;
            QuestionId = question.QuestionId;
            Order = order;
        }
    }
}
