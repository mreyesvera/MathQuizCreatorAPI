using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MathQuizCreatorAPI.Data;
using MathQuizCreatorAPI.Models;
using Microsoft.AspNetCore.Cors;
using MathQuizCreatorAPI.DTOs.Question;
using MathQuizCreatorAPI.DTOs.QuizQuestion;
using MathQuizCreatorAPI.DTOs.Topic;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace MathQuizCreatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TopicsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TopicsController(AppDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/Topics
        [HttpGet]
        [EnableCors("ReactApp")]
        public async Task<ActionResult<IEnumerable<TopicDeepDto>>> GetTopics(bool owner = false)
        {
            // TO DO: Later filter this by the user that is sending the request
            // also will need to check that they have the appropriate role?
            // maybe not, because if they are not a creator they won't have any quizzes

            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Guid guidUserId;

            if (userId == null || !Guid.TryParse(userId, out guidUserId))
            {
                return BadRequest("Unidentified user.");
            }


            var topics = await _context.Topics
                .Include(topic => topic.Questions)
                .ThenInclude(question => question.Creator)
                .Include(topic => topic.Quizzes).ToListAsync();


            var topicsDto = new List<TopicDeepDto>();

            foreach (var topic in topics)
            {
                var questionsDto = new List<QuestionSimplifiedDto>();
                foreach(var question in topic.Questions)
                {
                    if (question.Creator.Id == guidUserId)
                    {
                        questionsDto.Add(new QuestionSimplifiedDto()
                        {
                            QuestionId = question.QuestionId,
                            Title = question.Title,
                            Description = question.Description,
                            LastModifiedTime = question.LastModifiedTime,
                            CreationTime = question.CreationTime,
                            AssignedQuizzes = await QuestionsController.GetAssignedQuizzes(_context, question.QuestionId)
                        });
                    }
                }

                var quizzesDto = new List<QuizSimplifiedDto>();
                foreach(var quiz in topic.Quizzes)
                {
                    if(
                        (owner == true && quiz.Creator.Id == guidUserId) ||
                        (owner == false && (quiz.Creator.Id == guidUserId || quiz.IsPublic))
                        )
                    //if (quiz.Creator.Id == guidUserId || quiz.IsPublic)
                    {
                        quizzesDto.Add(new QuizSimplifiedDto()
                        {
                            QuizId = quiz.QuizId,
                            Title = quiz.Title,
                            Description = quiz.Description,
                            IsPublic = quiz.IsPublic,
                            HasUnlimitedMode = quiz.HasUnlimitedMode,
                            LastModifiedTime = quiz.LastModifiedTime,
                            CreationTime = quiz.CreationTime
                        });
                    }
                }
                var topicDto = new TopicDeepDto()
                {
                    TopicId = topic.TopicId,
                    Title = topic.Title,
                    Questions = questionsDto,
                    Quizzes = quizzesDto
                };

                topicsDto.Add(topicDto);
            }

            return topicsDto;
        }

        // GET: api/Topics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Topic>> GetTopic(Guid id)
        {
            var topic = await _context.Topics.FindAsync(id);


            if (topic == null)
            {
                return NotFound();
            }

            return topic;
        }

        // PUT: api/Topics/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTopic(Guid id, Topic topic)
        {
            if (id != topic.TopicId)
            {
                return BadRequest();
            }

            _context.Entry(topic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TopicExists(id))
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

        // POST: api/Topics
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Topic>> PostTopic(Topic topic)
        {
            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTopic", new { id = topic.TopicId }, topic);
        }

        // DELETE: api/Topics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic(Guid id)
        {
            var topic = await _context.Topics.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }

            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TopicExists(Guid id)
        {
            return _context.Topics.Any(e => e.TopicId == id);
        }
    }
}
