using MathQuizCreatorAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs
{
    public class TopicSimplifiedDto
    {
        [Required]
        public Guid TopicId { get; set; }

        [Required]
        public string Title { get; set; }

    }
}
