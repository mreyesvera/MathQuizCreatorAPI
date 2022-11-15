using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
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
    /// This model represents a Question. It holds what a topic content
    /// would include by deafult (title and description) as it inherits from it,
    /// and also includes the answer and creator. 
    /// </summary>
    [JsonObject]
    public class Question : TopicContent
    {
        [Key]
        public Guid QuestionId { get; set; }


        private string? _answer;

        [BackingField(nameof(_answer))]
        [MaxLength(500)]
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

        private ApplicationUser? _creator;

        [BackingField(nameof(_creator))]

        public ApplicationUser? Creator
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

        public Question(string title, string description, string answer, Topic topic, ApplicationUser creator)
        : base (title, description, topic)
        {
            QuestionId = Guid.NewGuid();
            Answer = answer;
            Creator = creator;
        }
    }
}
