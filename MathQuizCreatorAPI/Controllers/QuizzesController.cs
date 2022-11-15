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
    [Authorize(Roles = "Creator")]
    public class QuizzesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QuizzesController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Returns the question with the matching question id.
        /// </summary>
        /// <param name="questionId">question id of question to look for</param>
        /// <returns>found question simplified</returns>
        /*private async Task<QuestionSimplifiedDto> GetQuestionSimplified(Guid? questionId)
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
        }*/

        // Not needed at the moment
        // GET: api/Quizzes
        /*[HttpGet]
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
        }*/

        /// <summary>
        /// Returns the found quiz questions for a provided quizId.
        /// </summary>
        /// <param name="quizId">Quiz id to filter quiz questions for</param>
        /// <returns>List of quiz questions with the detailed quesiton</returns>
        /*private async Task<List<QuizQuestionQuestionDeepDto>> GetQuizQuestionsQuestionDeep(Guid quizId)
        {
            var quizQuestions = await _context.QuizQuestions.Where(quizQuestion => quizQuestion.QuizId == quizId).ToListAsync();

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
        }*/

        // GET: api/Quizzes/5
        /*[HttpGet("{id}")]
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
        }*/

        /// <summary>
        /// Updates a quiz for the provided id with the provided editted quiz.
        /// </summary>
        /// <param name="id">quiz id of quiz to update</param>
        /// <param name="quizEdit">quiz editted</param>
        /// <returns>no content if successful, error otherwise</returns>
        // PUT: api/Quizzes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        //[Authorize(Roles = "Creator")]
        public async Task<IActionResult> PutQuiz(Guid id, QuizEditDto quizEdit)
        {
            try
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }

        /// <summary>
        /// Adds a quiz.
        /// </summary>
        /// <param name="quizAdd">quiz to add</param>
        /// <returns>Added quiz</returns>
        // POST: api/Quizzes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //[Authorize(Roles = "Creator")]
        public async Task<ActionResult<QuizSimplifiedDto>> PostQuiz(QuizAddDto quizAdd)
        {
            try
            {
                Guid? topicId = quizAdd.TopicId;

                if (topicId == null)
                {
                    return BadRequest("Topic id can't be empty.");
                }

                Topic topic = await _context.Topics.Where(topic => topic.TopicId == topicId).SingleOrDefaultAsync();

                if (topic == null)
                {
                    return BadRequest("Topic couldn't be found with the given Topic Id.");
                }

                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Guid creatorId;

                if (userId == null || !Guid.TryParse(userId, out creatorId))
                {
                    return BadRequest("Unidentified user.");
                }

                ApplicationUser creator = await _context.Users.Where(creator => creator.Id == creatorId).SingleOrDefaultAsync();

                if (creator == null)
                {
                    return Unauthorized();
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

        /// <summary>
        /// Deletes a quiz from the provided id.
        /// </summary>
        /// <param name="id">quiz id of quiz to delete</param>
        /// <returns>no content if successful, error otherwise</returns>
        // DELETE: api/Quizzes/5
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Creator")]
        public async Task<IActionResult> DeleteQuiz(Guid id)
        {
            try
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }

        /// <summary>
        /// Returns whether a quiz exists or not.
        /// </summary>
        /// <param name="id">quiz id of quiz to look for</param>
        /// <returns>true if quiz found, false otherwise</returns>
        private bool QuizExists(Guid id)
        {
            return _context.Quizzes.Any(e => e.QuizId == id);
        }
    }
}
