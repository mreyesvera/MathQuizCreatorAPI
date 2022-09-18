using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace MathQuizCreatorAPI.Models
{
    public class TopicContent : Entity
    {
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

        private string? _description;

        [BackingField(nameof(_description))]
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
        [Required]
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

        [Required]
        public Guid TopicId { get; set; }

        public TopicContent(string title, string description, Topic topic) : base()
        {
            Title = title;
            Description = description;
            Topic = topic;
            TopicId = topic.TopicId;
        }
    }
}
