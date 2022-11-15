using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MathQuizCreatorAPI.Data;
using MathQuizCreatorAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MathQuizCreatorAPI.DTOs.QuizQuestion;

namespace MathQuizCreatorAPI.Controllers
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// This controller manages actions for Quiz Questions.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Creator")]
    public class QuizQuestionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QuizQuestionsController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // Not needed at the moment
        // GET: api/QuizQuestions
        /*[AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizQuestion>>> GetQuizQuestions()
        {
            return await _context.QuizQuestions.ToListAsync();
        }*/

        // Not needed at the moment
        // GET: api/QuizQuestions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuizQuestionSimplifiedDto>> GetQuizQuestion(Guid id)
        {
            try
            {
                var quizQuestion = await _context.QuizQuestions
                    .Where(quizQuestion => quizQuestion.QuizQuestionId == id)
                    .Include(quizQuestion => quizQuestion.Quiz)
                    .ThenInclude(quiz => quiz.Creator)
                    .FirstOrDefaultAsync();

                if (quizQuestion == null)
                {
                    return NotFound();
                }

                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Guid guidUserId;

                if (userId == null || !Guid.TryParse(userId, out guidUserId))
                {
                    return BadRequest("Unidentified user.");
                }

                if(quizQuestion.Quiz.Creator.Id != guidUserId)
                {
                    return Unauthorized();
                }

                var quizQuestionSimplified = new QuizQuestionSimplifiedDto()
                {
                    QuizQuestionId = quizQuestion.QuizQuestionId,
                    QuizId = quizQuestion.QuizId,
                    QuestionId = quizQuestion.QuestionId,
                    Order = quizQuestion.Order
                };

                return quizQuestionSimplified;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }

        /// <summary>
        /// Updates a provided quiz question. 
        /// </summary>
        /// <param name="id">id of the quiz question to update</param>
        /// <param name="quizQuestionEdit">updated quiz question</param>
        /// <returns>no content if successful, error otherwise</returns>
        // PUT: api/QuizQuestions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuizQuestion(Guid id, QuizQuestion quizQuestionEdit)
        {
            try
            {
                if (id != quizQuestionEdit.QuizId)
                {
                    return BadRequest();
                }

                var quizQuestion = await _context.QuizQuestions
                    .Where(quizQuestion => quizQuestion.QuizQuestionId == id)
                    .Include(quizQuestion => quizQuestion.Quiz)
                    .ThenInclude(quiz => quiz.Creator)
                    .Include(quizQuestion => quizQuestion.Question)
                    .ThenInclude(question => question.Creator)
                    .FirstOrDefaultAsync();

                if (quizQuestion == null)
                {
                    return NotFound();
                }

                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Guid guidUserId;

                if (userId == null || !Guid.TryParse(userId, out guidUserId))
                {
                    return BadRequest("Unidentified user.");
                }


                if (quizQuestion.Quiz == null || quizQuestion.Question == null)
                {
                    return BadRequest("Quiz or question couldn't be found with the provided Ids.");
                }

                if (quizQuestion.Quiz.Creator.Id != guidUserId || quizQuestion.Question.Creator.Id != guidUserId)
                {
                    return Unauthorized();
                }

                if(quizQuestion.Quiz.TopicId != quizQuestion.Question.TopicId)
                {
                    return BadRequest("Can't assign question of different topic to quiz.");
                }

                _context.Entry(quizQuestion).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuizQuestionExists(id))
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
        /// Adds a quiz question.
        /// </summary>
        /// <param name="quizQuestion">quiz question to add</param>
        /// <returns>added quiz question</returns>
        // POST: api/QuizQuestions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<QuizQuestionSimplifiedDto>> PostQuizQuestion(QuizQuestion quizQuestion)
        {
            try
            {
                if (quizQuestion.QuizId == null || quizQuestion.QuestionId == null)
                {
                    return BadRequest("Quiz and Question id's can't be empty.");
                }

                var quiz = await _context.Quizzes
                    .Where(quiz => quiz.QuizId == quizQuestion.QuizId)
                    .Include(quiz => quiz.Creator)
                    .SingleOrDefaultAsync();

                var question = await _context.Questions
                    .Where(question => question.QuestionId == quizQuestion.QuestionId)
                    .Include(question => question.Creator)
                    .SingleOrDefaultAsync();

                var quizQuestions = await _context.QuizQuestions
                    .Where(qq => qq.QuizId == quizQuestion.QuizId)
                    .Where(qq => qq.QuestionId == quizQuestion.QuestionId)
                    .ToListAsync();


                if (quiz == null || question == null)
                {
                    return BadRequest("Quiz or question couldn't be found with the provided ids.");
                }

                if(quizQuestions.Count != 0)
                {
                    return BadRequest("Can't add quiz question that already exists.");
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

                if(quiz.Creator.Id != creatorId || question.Creator.Id != creatorId)
                {
                    return Unauthorized();
                }

                if (quiz.TopicId != question.TopicId)
                {
                    return BadRequest("Can't assign question of different topic to quiz.");
                }

                _context.QuizQuestions.Add(quizQuestion);

                await _context.SaveChangesAsync();

                var quizQuestionSimplified = new QuizQuestionSimplifiedDto()
                {
                    QuizQuestionId = quizQuestion.QuizQuestionId,
                    QuizId = quizQuestion.QuizId,
                    QuestionId = quizQuestion.QuestionId,
                    Order = quizQuestion.Order
                };

                return CreatedAtAction("GetQuizQuestion", new { id = quizQuestion.QuizQuestionId }, quizQuestion);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }

        /// <summary>
        /// Deletes a quiz question matching the provided id
        /// </summary>
        /// <param name="id">id of quiz question to delete</param>
        /// <returns>no content if successful, error otherwise</returns>
        // DELETE: api/QuizQuestions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuizQuestion(Guid id)
        {
            try
            {
                var quizQuestion = await _context.QuizQuestions
                    .Include(quizQuestion => quizQuestion.Quiz)
                    .ThenInclude(quiz => quiz.Creator)
                    .Where(quizQuestion => quizQuestion.QuizQuestionId == id)
                    .FirstOrDefaultAsync();

                if (quizQuestion == null)
                {
                    return NotFound();
                }

                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Guid guidUserId;

                if (userId == null || !Guid.TryParse(userId, out guidUserId))
                {
                    return BadRequest("Unidentified user.");
                }

                if (quizQuestion.Quiz.Creator.Id != guidUserId)
                {
                    return Unauthorized();
                }

                _context.QuizQuestions.Remove(quizQuestion);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }

        /// <summary>
        /// Returns whether a quiz question exists or not.
        /// </summary>
        /// <param name="id">id of quiz question to look for</param>
        /// <returns>true if quiz question is found, false otherwise</returns>
        private bool QuizQuestionExists(Guid id)
        {
            return _context.QuizQuestions.Any(e => e.QuizQuestionId == id);
        }
    }
}
