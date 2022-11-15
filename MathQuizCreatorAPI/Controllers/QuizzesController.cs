using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MathQuizCreatorAPI.Data;
using MathQuizCreatorAPI.Models;
using System.Drawing;
using MathQuizCreatorAPI.DTOs;
using MathQuizCreatorAPI.DTOs.Topic;
using MathQuizCreatorAPI.DTOs.Question;
using MathQuizCreatorAPI.DTOs.Quiz;
using MathQuizCreatorAPI.DTOs.QuizQuestion;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MathQuizCreatorAPI.DTOs.Authentication;

namespace MathQuizCreatorAPI.Controllers
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// This controller manages actions for Quizzes.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuizzesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QuizzesController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<QuestionSimplifiedDto> GetQuestionSimplified(Guid? questionId)
        {
            var question = await _context.Questions.Where(question => question.QuestionId == questionId).FirstOrDefaultAsync();

            if(questionId == null)
            {
                return null;
            }

            var questionSimplified = new QuestionSimplifiedDto()
            {
                QuestionId = questionId ?? Guid.Empty,
                Title = question.Title,
                Description = question.Description,
                Answer = question.Answer,
                AssignedQuizzes = await QuestionsController.GetAssignedQuizzes(_context, questionId ?? Guid.Empty)
            };

            return questionSimplified;
        }

        // GET: api/Quizzes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizSimplifiedDto>>> GetQuizzes()
        {
            var quizzes = await _context.Quizzes.ToListAsync();

            var quizzesSimplified = new List<QuizSimplifiedDto>();

            foreach(var quiz in quizzes)
            {
                quizzesSimplified.Add(new QuizSimplifiedDto()
                {
                    QuizId = quiz.QuizId,
                    Title = quiz.Title,
                    Description = quiz.Description,
                    IsPublic = quiz.IsPublic,
                    HasUnlimitedMode = quiz.HasUnlimitedMode,
                });
            }

            return quizzesSimplified;
        }

        private async Task<List<QuizQuestionQuestionDeepDto>> GetQuizQuestionsQuestionDeep(Guid quizQuestionId)
        {
            var quizQuestions = await _context.QuizQuestions.Where(quizQuestion => quizQuestion.QuizId == quizQuestionId).ToListAsync();

            var quizQuestionsQuestions = new List<QuizQuestionQuestionDeepDto>();

            foreach(var quizQuestion in quizQuestions)
            {
                quizQuestionsQuestions.Add(new QuizQuestionQuestionDeepDto()
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

        // GET: api/Quizzes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuizDeepDto>> GetQuiz(Guid id)
        {
            var quiz = await _context.Quizzes
                .Where(quiz => quiz.QuizId == id)
                .Include(quiz => quiz.Topic)
                .Include(quiz => quiz.QuizQuestions)
                .Include(quiz => quiz.Creator)
                .FirstOrDefaultAsync();

            if (quiz == null)
            {
                return NotFound();
            }

            var quizDeep = new QuizDeepDto()
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
                    UserId = quiz.Creator.Id,
                    Email = quiz.Creator.Email,
                    UserName = quiz.Creator.UserName,
                },
                QuizQuestions = await GetQuizQuestionsQuestionDeep(quiz.QuizId)

            };


            return quizDeep;
        }

        // PUT: api/Quizzes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> PutQuiz(Guid id, QuizEditDto quizEdit)
        {
            if (id != quizEdit.QuizId)
            {
                return BadRequest();
            }

            var quiz = await _context.Quizzes
                .Where(quiz => quiz.QuizId == id)
                .Include(quiz => quiz.Creator)
                .FirstOrDefaultAsync();

            if (quiz == null)
            {
                return NotFound();
            }

            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Guid guidUserId;

            if (userId == null || !Guid.TryParse(userId, out guidUserId))
            {
                return BadRequest("Unidentified user.");
            }

            if (quiz.Creator.Id != guidUserId)
            {
                return Unauthorized();
            }

            quiz.Title = quizEdit.Title;
            quiz.Description = quizEdit.Description;
            quiz.IsPublic = quizEdit.IsPublic;
            quiz.HasUnlimitedMode = quizEdit.HasUnlimitedMode;

            _context.Entry(quiz).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Quizzes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Creator")]
        public async Task<ActionResult<QuizSimplifiedDto>> PostQuiz(QuizAddDto quizAdd)
        {
            try
            {
                Guid? topicId = quizAdd.TopicId;

                if (topicId == null)
                {
                    throw new ArgumentException("Topic id can't be empty.");
                }

                Topic topic = await _context.Topics.Where(topic => topic.TopicId == topicId).SingleOrDefaultAsync();

                if (topic == null)
                {
                    throw new ArgumentException("Topic couldn't be found with the given Topic Id.");
                }

                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Guid creatorId;
                //Guid? creatorId = quizAdd.CreatorId;

                if (userId == null || !Guid.TryParse(userId, out creatorId))
                {
                    //throw new ArgumentException("Creatr id can't be empty.");
                    throw new ArgumentException("Unidentified user.");
                }

                ApplicationUser creator = await _context.Users.Where(creator => creator.Id == creatorId).SingleOrDefaultAsync();

                if (creator == null)
                {
                    throw new ArgumentException("Creator couldn't be found with the given Creator Id.");
                }

                var quiz = new Quiz()
                {
                    Title = quizAdd.Title,
                    Description = quizAdd.Description,
                    IsPublic = quizAdd.IsPublic,
                    HasUnlimitedMode = quizAdd.HasUnlimitedMode,
                    Topic = topic,
                    Creator = creator,
                };

                _context.Quizzes.Add(quiz);
                await _context.SaveChangesAsync();

                var quizSimplified = new QuizSimplifiedDto()
                {
                    QuizId = quiz.QuizId,
                    Title = quiz.Title,
                    Description = quiz.Description,
                    IsPublic = quiz.IsPublic,
                    HasUnlimitedMode = quiz.HasUnlimitedMode,
                };

                return CreatedAtAction("GetQuiz", new { id = quiz.QuizId }, quizSimplified);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }

        // DELETE: api/Quizzes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> DeleteQuiz(Guid id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Guid guidUserId;

            if (userId == null || !Guid.TryParse(userId, out guidUserId))
            {
                return BadRequest("Unidentified user.");
            }

            if (quiz.Creator.Id != guidUserId)
            {
                return Unauthorized();
            }

            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuizExists(Guid id)
        {
            return _context.Quizzes.Any(e => e.QuizId == id);
        }
    }
}
