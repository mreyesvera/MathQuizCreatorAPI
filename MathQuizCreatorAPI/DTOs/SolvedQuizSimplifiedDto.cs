using MathQuizCreatorAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs
{
    public class SolvedQuizSimplifiedDto
    {
        [Required]
        public Guid SolvedQuizId { get; set; }

        [Required]
        public int CorrectResponses { get; set; }

        [Required]
        public int IncorrectResponses { get; set; }

        [Required]
        public int TotalQuestions { get; set; }

        [Required]
        public double Score { get; set; }
    }
}
