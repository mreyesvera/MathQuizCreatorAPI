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
    /// This model represents a Topic Content, like Question or Quiz. 
    /// It holds the topic id as well as the content title and
    /// description.
    /// </summary>
    public class TopicContent : Entity
    {
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

        private string? _description;

        [BackingField(nameof(_description))]
        [MaxLength(500)]
        [Required]
        public string? Description
        {
            get => _description;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Description can't be null");
                }

                _description = value;
            }
        }

        private Topic? _topic;

        [BackingField(nameof(_topic))]
        public Topic? Topic
        {
            get => _topic;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Topic can't be null");
                }

                _topic = value;
            }
        }

        public Guid? TopicId { get; set; }

        public TopicContent()
        {

        }

        public TopicContent(string title, string description, Topic topic) : base()
        {
            Title = title;
            Description = description;
            Topic = topic;
            TopicId = topic.TopicId;
        }
    }
}
