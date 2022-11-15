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
    /// This model represents a Topic. It holds the topic's title as well
    /// as optionally any quizzes or questions that are under that topic. 
    /// </summary>
    [JsonObject]
    public class Topic : Entity
    {
        [Key]
        public Guid TopicId { get; set; }

        private string? _title;

        [BackingField(nameof(_title))]
        [MaxLength(200)]
        [Required]
        public string? Title
        {
            get => _title;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Title can't be null");
                }

                _title = value;
            }
        }


        public List<Quiz>? Quizzes { get; set; }

        public List<Question>? Questions { get; set; }


        public Topic()
        {

        }

        public Topic(string title) : base()
        {
            TopicId = Guid.NewGuid();
            Title = title;
            Quizzes = new List<Quiz>();
            Questions = new List<Question>();
        }
    }
}
