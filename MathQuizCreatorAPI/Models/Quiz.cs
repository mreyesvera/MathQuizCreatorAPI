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



        public List<QuizQuestion>? QuizQuestions { get; set; }


        public Quiz()
        {

        }

        public Quiz(string title, string description, bool isPublic, bool hasUnlimitedMode, Topic topic)
        : base(title, description, topic)
        {
            QuizId = Guid.NewGuid();
            IsPublic = isPublic;
            HasUnlimitedMode = hasUnlimitedMode;
        }

    }
}
