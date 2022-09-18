using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MathQuizCreatorAPI.Models
{
    [JsonObject]
    public class Topic : Entity
    {
        [Key]
        public Guid TopicId { get; set; }

        private string? _title;

        [BackingField(nameof(_title))]
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


        public List<Quiz> Quizzes { get; set; }

        public List<Question> Questions { get; set; }



        public Topic(string title) : base()
        {
            TopicId = Guid.NewGuid();
            Title = title;
            Quizzes = new List<Quiz>();
            Questions = new List<Question>();
        }
    }
}
