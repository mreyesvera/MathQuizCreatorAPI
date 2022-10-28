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
using Microsoft.AspNetCore.Cors;

namespace MathQuizCreatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TopicsController(AppDbContext context)
        {
            _context = context;
        }

        private async Task<string> GetAssignedQuizzes(Guid id)
        {
            var quizQuestions = await _context.QuizQuestions
                .Include(quizQuestion => quizQuestion.Quiz)
                .Where(quizQuestion => quizQuestion.QuestionId == id).ToListAsync();
            string assignedQuizzes = "";

            foreach (var quizQuestion in quizQuestions)
            {
                assignedQuizzes += $"{quizQuestion.Quiz.Title}\n";
            }

            return assignedQuizzes;
        }

        // GET: api/Topics
        [HttpGet]
        [EnableCors("ReactApp")]
        public async Task<ActionResult<IEnumerable<TopicDeepDto>>> GetTopics()
        {
            // TO DO: Later filter this by the user that is sending the request
            // also will need to check that they have the appropriate role?
            // maybe not, because if they are not a creator they won't have any quizzes
            var topics = await _context.Topics
                .Include(topic => topic.Questions)
                .Include(topic => topic.Quizzes).ToListAsync();

            var topicsDto = new List<TopicDeepDto>();

            foreach (var topic in topics)
            {
                var questionsDto = new List<QuestionSimplifiedDto>();
                foreach(var question in topic.Questions)
                {
                    questionsDto.Add(new QuestionSimplifiedDto()
                    {
                        QuestionId = question.QuestionId,
                        Title = question.Title,
                        Description = question.Description,
                        LastModifiedTime = question.LastModifiedTime,
                        CreationTime = question.CreationTime,
                        AssignedQuizzes = await GetAssignedQuizzes(question.QuestionId)
                    });
                }

                var quizzesDto = new List<QuizSimplifiedDto>();
                foreach(var quiz in topic.Quizzes)
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
