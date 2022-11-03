using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MathQuizCreatorAPI.Data;
using MathQuizCreatorAPI.Models;
using MathQuizCreatorAPI.DTOs;

namespace MathQuizCreatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizzesLearnerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QuizzesLearnerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/QuizzesLearner
        /*[HttpGet]
        public async Task<ActionResult<IEnumerable<Quiz>>> GetQuizzes()
        {
            return await _context.Quizzes.ToListAsync();
        }*/

        private async Task<QuestionSimplifiedSafeDto> GetQuestionSimplified(Guid? questionId)
        {
            var question = await _context.Questions.Where(question => question.QuestionId == questionId).FirstOrDefaultAsync();

            if (questionId == null)
            {
                return null;
            }

            var questionSimplified = new QuestionSimplifiedSafeDto()
            {
                QuestionId = questionId ?? Guid.Empty,
                Title = question.Title,
                Description = question.Description,
                AssignedQuizzes = await QuestionsController.GetAssignedQuizzes(_context, questionId ?? Guid.Empty)
            };

            return questionSimplified;
        }

        private async Task<List<QuizQuestionQuestionDeepSafeDto>> GetQuizQuestionsQuestionDeep(Guid quizQuestionId)
        {
            var quizQuestions = await _context.QuizQuestions.Where(quizQuestion => quizQuestion.QuizId == quizQuestionId).ToListAsync();

            var quizQuestionsQuestions = new List<QuizQuestionQuestionDeepSafeDto>();

            foreach (var quizQuestion in quizQuestions)
            {
                quizQuestionsQuestions.Add(new QuizQuestionQuestionDeepSafeDto()
                {
                    QuizQuestionId = quizQuestion.QuizQuestionId,
                    QuizId = quizQuestion.QuizId,
                    QuestionId = quizQuestion.QuestionId,
                    Question = await GetQuestionSimplified(quizQuestion.QuestionId),
                    Order = quizQuestion.Order
                });
            }

            return quizQuestionsQuestions;
        }

        // GET: api/QuizzesLearner/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuizDeepSafeDto>> GetQuiz(Guid id)
        {
            var quiz = await _context.Quizzes
                .Where(quiz => quiz.QuizId == id)
                .Include(quiz => quiz.Topic)
                .Include(quiz => quiz.QuizQuestions)
                .Include(quiz => quiz.Creator)
                .Include(quiz => quiz.Creator.Role)
                .FirstOrDefaultAsync();

            if (quiz == null)
            {
                return NotFound();
            }

            var quizDeep = new QuizDeepSafeDto()
            {
                QuizId = quiz.QuizId,
                Title = quiz.Title,
                Description = quiz.Description,
                IsPublic = quiz.IsPublic,
                HasUnlimitedMode = quiz.HasUnlimitedMode,
                LastModifiedTime = quiz.LastModifiedTime,
                CreationTime = quiz.CreationTime,
                Topic = new TopicSimplifiedDto()
                {
                    TopicId = quiz.Topic.TopicId,
                    Title = quiz.Topic.Title
                },
                Creator = new UserSimplifiedDto()
                {
                    UserId = quiz.Creator.UserId,
                    Email = quiz.Creator.Email,
                    Username = quiz.Creator.Username,
                    Role = new RoleSimplifiedDto()
                    {
                        RoleId = quiz.Creator.Role.RoleId,
                        Title = quiz.Creator.Role.Title
                    },
                },
                QuizQuestions = await GetQuizQuestionsQuestionDeep(quiz.QuizId)

            };


            return quizDeep;
        }
    }
}
