﻿using MathQuizCreatorAPI.DTOs.Authentication;
using MathQuizCreatorAPI.DTOs.QuizQuestion;
using MathQuizCreatorAPI.DTOs.Topic;
using MathQuizCreatorAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MathQuizCreatorAPI.DTOs.Quiz
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Question Deep Safe Dto. Same as QuizDeepDto, 
    /// but using the QuestionSimplifiedSafeDto. 
    /// </summary>
    public class QuizDeepSafeDto : Entity
    {
        [Required]
        public Guid QuizId { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        [Required]
        public bool HasUnlimitedMode { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public UserSimplifiedDto Creator { get; set; }

        public List<QuizQuestionQuestionDeepSafeDto>? QuizQuestions { get; set; }

        public TopicSimplifiedDto? Topic { get; set; }
    }
}
