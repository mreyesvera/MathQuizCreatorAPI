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

namespace MathQuizCreatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QuestionsController(AppDbContext context)
        {
            _context = context;
        }

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
            var question = await _context.Questions
                .Where(question => question.QuestionId == id)
                .Include(question => question.Topic)
                .FirstOrDefaultAsync();

            if (question == null)
            {
                return NotFound();
            }

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
                CreationTime = question.CreationTime
            };

            return questionDeep;
        }

        // PUT: api/Questions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestion(Guid id, QuestionEditDto questionEdit)
        {
            if (id != questionEdit.QuestionId)
            {
                return BadRequest();
            }

            var question = await _context.Questions.Where(question => question.QuestionId == id).FirstOrDefaultAsync();

            if (question == null)
            {
                return NotFound();
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
        public async Task<ActionResult<QuestionSimplifiedDto>> PostQuestion(QuestionAddDto questionAdd)
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

            Guid? creatorId = questionAdd.CreatorId;

            if (creatorId == null)
            {
                throw new ArgumentException("Creator id can't be empty.");
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

        // DELETE: api/Questions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(Guid id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
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
