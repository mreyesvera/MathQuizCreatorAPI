using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs
{
    public class RoleSimplifiedDto
    {
        [Required]
        public Guid RoleId { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
