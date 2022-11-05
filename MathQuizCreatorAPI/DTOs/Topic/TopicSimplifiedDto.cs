using MathQuizCreatorAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs.Topic
{
    public class TopicSimplifiedDto
    {
        [Required]
        public Guid TopicId { get; set; }

        [Required]
        public string Title { get; set; }

    }
}
