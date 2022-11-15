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
    [Authorize]
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
        //[Authorize]
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
        /// Returns a list of questions. If the topic id is provided,
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        // GET: api/Questions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionSimplifiedDto>>> GetQuestions(Guid? topicId = null)
        {
            var questions = await _context.Questions
                .Include(question => question.Topic)
                .Include(question => question.QuizQuestions)
                .Where(question => (topicId == null || question.Topic.TopicId == topicId)).ToListAsync();

            var questionsSimplified = new List<QuestionSimplifiedDto>();

            foreach(var question in questions)
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

        // GET: api/Questions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDeepDto>> GetQuestion(Guid id)
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

        // PUT: api/Questions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> PutQuestion(Guid id, QuestionEditDto questionEdit)
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

            if(question.Creator.Id != guidUserId)
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

        // POST: api/Questions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Creator")]
        public async Task<ActionResult<QuestionSimplifiedDto>> PostQuestion(QuestionAddDto questionAdd)
        {
            try
            {
                Guid? topicId = questionAdd.TopicId;

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
                //Guid? creatorId = questionAdd.CreatorId;

                if (userId == null || !Guid.TryParse(userId, out creatorId))
                {
                    //throw new ArgumentException("Creator id can't be empty.");
                    throw new ArgumentException("Unidentified user.");
                }

                ApplicationUser creator = await _context.Users.Where(creator => creator.Id == creatorId).SingleOrDefaultAsync();

                if (creator == null)
                {
                    throw new ArgumentException("Creator couldn't be found with the given Creator Id.");
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

        // DELETE: api/Questions/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> DeleteQuestion(Guid id)
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

        private bool QuestionExists(Guid id)
        {
            return _context.Questions.Any(e => e.QuestionId == id);
        }
    }
}
