using MathQuizCreatorAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs
{
    public class UserSimplifiedDto 
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public RoleSimplifiedDto? Role { get; set; }
    }
}
