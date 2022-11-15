using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs.Quiz
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Quiz Add Dto. Used when receiving quiz data to add.
    /// </summary>
    public class QuizAddDto
    {
        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        [Required]
        public bool HasUnlimitedMode { get; set; }

        [Required]
        public Guid TopicId { get; set; }

        //[Required]
        //public Guid CreatorId { get; set; }
    }
}
