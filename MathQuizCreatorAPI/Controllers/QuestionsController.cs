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
    public class QuestionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QuestionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Questions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestions()
        {
            return await _context.Questions.ToListAsync();
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
        public async Task<ActionResult<Question>> PostQuestion(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestion", new { id = question.QuestionId }, question);
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
