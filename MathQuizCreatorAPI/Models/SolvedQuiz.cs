using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MathQuizCreatorAPI.Models
{
    [JsonObject]
    public class SolvedQuiz : Entity
    {
        [Key]
        public Guid SolvedQuizId { get; set; }



        public Guid? UserId { get; set; }

        private ApplicationUser? _user;

        [BackingField(nameof(_user))]
        public ApplicationUser? User
        {
            get => _user;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("User can't be null");
                }

                _user = value;
            }
        }

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


        private int _correctResponses;

        [BackingField(nameof(_correctResponses))]
        [Required]
        public int CorrectResponses
        {
            get => _correctResponses;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Correct responses can't be negative");
                }

                _correctResponses = value;
                TotalQuestions = IncorrectResponses + _correctResponses;
                Score = (double) _correctResponses / TotalQuestions;
            }
        }

        private int _incorrectResponses;

        [BackingField(nameof(_incorrectResponses))]
        [Required]
        public int IncorrectResponses
        {
            get => _incorrectResponses;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Incorrect responses can't be negative");
                }

                _incorrectResponses = value;
                TotalQuestions = CorrectResponses + _incorrectResponses;
                Score = (double)CorrectResponses / TotalQuestions;
            }
        }

        [Required]
        public int TotalQuestions { get; set; }

        [Required]
        public double Score { get; set; }


        public SolvedQuiz()
        {

        }

        public SolvedQuiz(ApplicationUser user, Quiz quiz, int correctResponses, int incorrectResponses)
        : base()
        {
            SolvedQuizId = Guid.NewGuid();
            User = user;
            UserId = user.Id;
            Quiz = quiz;
            QuizId = quiz.QuizId;
            CorrectResponses = correctResponses;
            IncorrectResponses = incorrectResponses;
            TotalQuestions = correctResponses + incorrectResponses;
            Score = (double)correctResponses / TotalQuestions;
        }
    }
}
