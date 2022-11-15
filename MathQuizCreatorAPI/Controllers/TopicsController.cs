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
using MathQuizCreatorAPI.DTOs.Topic;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MathQuizCreatorAPI.DTOs.Quiz;
using MathQuizCreatorAPI.DTOs.SolvedQuiz;

namespace MathQuizCreatorAPI.Controllers
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// This controller manages actions for Topics.
    /// </summary>
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
        //[EnableCors("ReactApp")]
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
                foreach (var question in topic.Questions)
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
                foreach (var quiz in topic.Quizzes)
                {
                    if (
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

        // GET: api/Topics
        [HttpGet("SolvedQuizzes")]
        public async Task<ActionResult<IEnumerable<TopicSolvedQuizzesDto>>> GetTopicsSolvedQuizzes()
        {
            if(_httpContextAccessor.HttpContext == null 
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

            var topics = await _context.Topics
                .Include(topic => topic.Quizzes)
                .ToListAsync();

            if(topics == null || topics.Count == 0)
            {
                return NoContent();
            }

            var topicsSolvedQuizzes = new List<TopicSolvedQuizzesDto>();
            for(int i=0; i<topics.Count; i++)
            {
                var topic = topics[i];
                var topicQuizzes = topic.Quizzes.Select(quiz => quiz.QuizId).ToList();

                var solvedQuizzes = await _context.SolvedQuizzes
                        .Include(solvedQuiz => solvedQuiz.Quiz)
                        .Include(solvedQuiz => solvedQuiz.User)
                        .Where(solvedQuiz => solvedQuiz.Quiz.TopicId == topic.TopicId)
                        .Where(solvedQuiz => solvedQuiz.Quiz.IsPublic)
                        .Where(solvedQuiz => solvedQuiz.UserId == guidUserId)
                        .Where(solvedQuiz => solvedQuiz.QuizId != null && topicQuizzes.Contains(solvedQuiz.QuizId ?? Guid.Empty))
                        .ToListAsync();

                var quizzesSolvedQuizzes = new List<QuizSolvedQuizzesDto>();

                for(int j=0; j<solvedQuizzes.Count; j++)
                {
                    var solvedQuiz = solvedQuizzes[j];
                    var solvedQuizSimplified = new SolvedQuizSimplifiedDto()
                    {
                        SolvedQuizId = solvedQuiz.SolvedQuizId,
                        CorrectResponses = solvedQuiz.CorrectResponses,
                        IncorrectResponses = solvedQuiz.IncorrectResponses,
                        TotalQuestions = solvedQuiz.TotalQuestions,
                        Score = solvedQuiz.Score
                    };

                    var quizSolvedQuizzes = quizzesSolvedQuizzes
                        .Where(quizSolvedQuiz => quizSolvedQuiz.QuizId == solvedQuiz.QuizId)
                        .ToList();

                    QuizSolvedQuizzesDto quizSolvedQuiz;
                    if(quizSolvedQuizzes == null || quizSolvedQuizzes.Count == 0)
                    {
                        quizSolvedQuiz = new QuizSolvedQuizzesDto()
                        {
                            QuizId = solvedQuiz.Quiz.QuizId,
                            IsPublic = solvedQuiz.Quiz.IsPublic,
                            HasUnlimitedMode = solvedQuiz.Quiz.HasUnlimitedMode,
                            Title = solvedQuiz.Quiz.Title,
                            Description = solvedQuiz.Quiz.Description,
                            SolvedQuizzes = new List<SolvedQuizSimplifiedDto>()
                        };

                        quizSolvedQuiz.SolvedQuizzes.Add(solvedQuizSimplified);

                        quizzesSolvedQuizzes.Add(quizSolvedQuiz);
                    } 
                    else
                    {
                        quizSolvedQuiz = quizSolvedQuizzes[0];
                        quizSolvedQuiz.SolvedQuizzes.Add(solvedQuizSimplified);
                    }
                }


                var topicSolvedQuiz = new TopicSolvedQuizzesDto()
                {
                    TopicId = topic.TopicId,
                    Title = topic.Title,
                    Quizzes = quizzesSolvedQuizzes
                };

                topicsSolvedQuizzes.Add(topicSolvedQuiz);

            }

            return topicsSolvedQuizzes;

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
