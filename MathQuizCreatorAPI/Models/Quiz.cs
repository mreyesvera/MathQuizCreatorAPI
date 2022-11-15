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
    /// This model represents a Quiz. It holds what a topic content
    /// would include by deafult (title and description) as it inherits from it,
    /// and also whether the quiz is public or not, if it has unlimited mode,
    /// the creator and can optionally hold the quiz questions.
    /// </summary>
    [JsonObject]
    public class Quiz : TopicContent
    {
        [Key]
        public Guid QuizId { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        [Required]
        public bool HasUnlimitedMode { get; set; }

        private ApplicationUser? _creator;

        [BackingField(nameof(_creator))]

        public ApplicationUser? Creator {
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

        public Quiz(string title, string description, bool isPublic, bool hasUnlimitedMode, Topic topic, ApplicationUser creator)
        : base(title, description, topic)
        {
            QuizId = Guid.NewGuid();
            IsPublic = isPublic;
            HasUnlimitedMode = hasUnlimitedMode;
            Creator = creator;
        }

    }
}
