using MathQuizCreatorAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs.SolvedQuiz
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Solved Quiz Simplfied Dto. Used when sending data without
    /// the need for the values of relationships. Used to avoid errors due to the 
    /// circular reference in JSON. 
    /// </summary>
    public class SolvedQuizSimplifiedDto : Entity
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
