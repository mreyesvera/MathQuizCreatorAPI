using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MathQuizCreatorAPI.Models
{
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
