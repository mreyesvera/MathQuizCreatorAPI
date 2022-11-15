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
using MathQuizCreatorAPI.DTOs.Question;
using MathQuizCreatorAPI.DTOs.Topic;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MathQuizCreatorAPI.DTOs.Parameter;

namespace MathQuizCreatorAPI.Controllers
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// This controller manages actions for Questions.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Creator")]
    public class QuestionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QuestionsController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets the assigned quizzes for a specific question in the provided context.  
        /// </summary>
        /// <param name="_context">Context used to get the assigned quizzes from</param>
        /// <param name="questionId">Question id to retrieve the assigned quizzes from</param>
        /// <returns>list of strings with the assigned quizzes' titles</returns>
        public static async Task<List<string>> GetAssignedQuizzes(AppDbContext _context, Guid questionId)
        {
            var quizQuestions = await _context.QuizQuestions
                .Include(quizQuestion => quizQuestion.Quiz)
                .Where(quizQuestion => quizQuestion.QuestionId == questionId).ToListAsync();
            
            var assignedQuizzes = new List<string>();

            foreach (var quizQuestion in quizQuestions)
            {
                assignedQuizzes.Add(quizQuestion.Quiz.Title);
            }

            return assignedQuizzes;
        }

        /// <summary>
        /// Returns a list of questions created by the user. 
        /// If the topic id is provided, it only returns the questions
        /// for the specified topic. 
        /// </summary>
        /// <param name="topicId">Topic Id used to filter out the returned questions</param>
        /// <returns>list of questions simplified</returns>
        // GET: api/Questions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionSimplifiedDto>>> GetQuestions(Guid? topicId = null)
        {
            try
            {
                if (_httpContextAccessor.HttpContext == null
                || _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == null)
                {
                    return Unauthorized();
                }

                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Guid guidUserId;

                if (userId == null || !Guid.TryParse(userId, out guidUserId))
                {
                    return BadRequest("Unidentified user.");
                }

                var questions = await _context.Questions
                    .Include(question => question.Topic)
                    .Include(question => question.QuizQuestions)
                    .Include(question => question.Creator)
                    .Where(question => (topicId == null || question.Topic.TopicId == topicId))
                    .Where(question => question.Creator.Id == guidUserId)
                    .ToListAsync();

                var questionsSimplified = new List<QuestionSimplifiedDto>();

                foreach (var question in questions)
                {
                    questionsSimplified.Add(new QuestionSimplifiedDto()
                    {
                        QuestionId = question.QuestionId,
                        Title = question.Title,
                        Description = question.Description,
                        Answer = question.Answer,
                        AssignedQuizzes = await GetAssignedQuizzes(_context, question.QuestionId),
                        LastModifiedTime = question.LastModifiedTime,
                        CreationTime = question.CreationTime,
                    });
                }

                return questionsSimplified;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }

        /// <summary>
        /// Returns a question created by the user based on the provided id. 
        /// </summary>
        /// <param name="id">Question id to look for</param>
        /// <returns>found question</returns>
        // GET: api/Questions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDeepDto>> GetQuestion(Guid id)
        {
            try
            {
                if (_httpContextAccessor.HttpContext == null
                || _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == null)
                {
                    return Unauthorized();
                }

                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Guid guidUserId;

                if (userId == null || !Guid.TryParse(userId, out guidUserId))
                {
                    return BadRequest("Unidentified user.");
                }

                var question = await _context.Questions
                    .Include(question => question.Topic)
                    .Include(question => question.Creator)
                    .Where(question => question.QuestionId == id)
                    .Where(question => question.Creator.Id == guidUserId)
                    .FirstOrDefaultAsync();

                if (question == null)
                {
                    return NotFound();
                }

                var parameters = await _context.Parameters
                                    .Include(param => param.Question)
                                    .ThenInclude(question => question.Creator)
                                    .Where(param => param.QuestionId == id)
                                    .Where(param => param.Question.Creator.Id == guidUserId)
                                    .ToListAsync();

                var parametersSimplified = new List<ParameterSimplifiedDto>();

                parameters.ForEach(param =>
                {
                    parametersSimplified.Add(new ParameterSimplifiedDto()
                    {
                        ParameterId = param.ParameterId,
                        Name = param.Name,
                        Value = param.Value,
                        Order = param.Order,
                        QuestionId = param.QuestionId
                    });
                });

                var questionDeep = new QuestionDeepDto()
                {
                    QuestionId = question.QuestionId,
                    Title = question.Title,
                    Description = question.Description,
                    Answer = question.Answer,
                    Topic = new TopicSimplifiedDto()
                    {
                        TopicId = question.Topic.TopicId,
                        Title = question.Topic.Title
                    },
                    LastModifiedTime = question.LastModifiedTime,
                    CreationTime = question.CreationTime,
                    Parameters = parametersSimplified
                };

                return questionDeep;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }

        /// <summary>
        /// Updates a question from the provided id, with the editted question data. 
        /// </summary>
        /// <param name="id">question id of question to update</param>
        /// <param name="questionEdit">question updated</param>
        /// <returns>no content if succeeded or an error otherwise</returns>
        // PUT: api/Questions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        //[Authorize(Roles = "Creator")]
        public async Task<IActionResult> PutQuestion(Guid id, QuestionEditDto questionEdit)
        {
            try
            {
                if (id != questionEdit.QuestionId)
                {
                    return BadRequest();
                }

                var question = await _context.Questions
                    .Where(question => question.QuestionId == id)
                    .Include(question => question.Creator)
                    .FirstOrDefaultAsync();

                if (question == null)
                {
                    return NotFound();
                }


                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Guid guidUserId;

                if (userId == null || !Guid.TryParse(userId, out guidUserId))
                {
                    return BadRequest("Unidentified user.");
                }

                if (question.Creator.Id != guidUserId)
                {
                    return Unauthorized();
                }


                question.Title = questionEdit.Title;
                question.Description = questionEdit.Description;
                question.Answer = questionEdit.Answer;

                _context.Entry(question).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(id))
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
        /// Creates a new question for the user given the provided added question.
        /// </summary>
        /// <param name="questionAdd">added question data</param>
        /// <returns>Created question</returns>
        // POST: api/Questions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //[Authorize(Roles = "Creator")]
        public async Task<ActionResult<QuestionSimplifiedDto>> PostQuestion(QuestionAddDto questionAdd)
        {
            try
            {
                Guid? topicId = questionAdd.TopicId;

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

                var question = new Question()
                {
                    Title = questionAdd.Title,
                    Description = questionAdd.Description,
                    Answer = questionAdd.Answer,
                    Topic = topic,
                    Creator = creator,
                };

                _context.Questions.Add(question);
                await _context.SaveChangesAsync();

                var questionSimplified = new QuestionSimplifiedDto()
                {
                    QuestionId = question.QuestionId,
                    Title = question.Title,
                    Description = question.Description,
                    Answer = question.Answer,
                    AssignedQuizzes = await GetAssignedQuizzes(_context, question.QuestionId)
                };


                return CreatedAtAction("GetQuestion", new { id = question.QuestionId }, questionSimplified);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }

        /// <summary>
        /// Deletes a question with the provided id.
        /// </summary>
        /// <param name="id">question id of question to delete</param>
        /// <returns>no content if succeeded or error otherwise</returns>
        // DELETE: api/Questions/5
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Creator")]
        public async Task<IActionResult> DeleteQuestion(Guid id)
        {
            try
            {
                var question = await _context.Questions.FindAsync(id);
                if (question == null)
                {
                    return NotFound();
                }

                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Guid guidUserId;

                if (userId == null || !Guid.TryParse(userId, out guidUserId))
                {
                    return BadRequest("Unidentified user.");
                }

                if (question.Creator.Id != guidUserId)
                {
                    return Unauthorized();
                }


                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }

        /// <summary>
        /// Returns whether a question exists based on a provided id.
        /// </summary>
        /// <param name="id">question id of question to look for</param>
        /// <returns>true if question is found, false otherwise</returns>
        private bool QuestionExists(Guid id)
        {
            return _context.Questions.Any(e => e.QuestionId == id);
        }
    }
}
