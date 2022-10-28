using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MathQuizCreatorAPI.Models
{
    [JsonObject]
    public class Question : TopicContent
    {
        [Key]
        public Guid QuestionId { get; set; }


        private string? _answer;

        [BackingField(nameof(_answer))]
        [Required]
        public string? Answer
        {
            get => _answer;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Answer can't be null");
                }

                _answer = value;
            }
        }

        private User? _creator;

        [BackingField(nameof(_creator))]

        public User? Creator
        {
            get => _creator;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Creator can't be null");
                }

                _creator = value;
            }
        }

        public List<QuizQuestion>? QuizQuestions { get; set; }

        public Question()
        {

        }

        public Question(string title, string description, string answer, Topic topic)
        : base (title, description, topic)
        {
            QuestionId = Guid.NewGuid();
            Answer = answer;
        }
    }
}
