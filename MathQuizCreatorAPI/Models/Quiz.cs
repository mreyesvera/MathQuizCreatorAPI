using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace MathQuizCreatorAPI.Models
{
    [JsonObject]
    public class Quiz : TopicContent
    {
        [Key]
        public Guid QuizId { get; set; }

        

        [Required]
        public bool IsPublic { get; set; }

        [Required]
        public bool HasUnlimitedMode { get; set; }

        private User? _creator;

        [BackingField(nameof(_creator))]

        public User? Creator {
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


        public Quiz()
        {

        }

        public Quiz(string title, string description, bool isPublic, bool hasUnlimitedMode, Topic topic, User creator)
        : base(title, description, topic)
        {
            QuizId = Guid.NewGuid();
            IsPublic = isPublic;
            HasUnlimitedMode = hasUnlimitedMode;
            Creator = creator;
        }

    }
}
