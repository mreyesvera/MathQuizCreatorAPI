using MathQuizCreatorAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs.QuizQuestion
{
    public class QuizQuestionQuizDeepDto
    {
        [Required]
        public Guid QuizQuestionId { get; set; }

        [Required]
        public Guid? QuestionId { get; set; }

        [Required]
        public Guid? QuizId { get; set; }

        [Required]
        public QuizSimplifiedDto? Quiz { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
