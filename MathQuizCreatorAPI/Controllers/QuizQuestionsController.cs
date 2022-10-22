using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MathQuizCreatorAPI.Data;
using MathQuizCreatorAPI.Models;

namespace MathQuizCreatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizQuestionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QuizQuestionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/QuizQuestions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizQuestion>>> GetQuizQuestions()
        {
            return await _context.QuizQuestions.ToListAsync();
        }

        // GET: api/QuizQuestions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuizQuestion>> GetQuizQuestion(Guid id)
        {
            var quizQuestion = await _context.QuizQuestions.FindAsync(id);

            if (quizQuestion == null)
            {
                return NotFound();
            }

            return quizQuestion;
        }

        // PUT: api/QuizQuestions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuizQuestion(Guid id, QuizQuestion quizQuestion)
        {
            if (id != quizQuestion.QuizQuestionId)
            {
                return BadRequest();
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

        // POST: api/QuizQuestions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<QuizQuestion>> PostQuizQuestion(QuizQuestion quizQuestion)
        {
            _context.QuizQuestions.Add(quizQuestion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuizQuestion", new { id = quizQuestion.QuizQuestionId }, quizQuestion);
        }

        // DELETE: api/QuizQuestions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuizQuestion(Guid id)
        {
            var quizQuestion = await _context.QuizQuestions.FindAsync(id);
            if (quizQuestion == null)
            {
                return NotFound();
            }

            _context.QuizQuestions.Remove(quizQuestion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuizQuestionExists(Guid id)
        {
            return _context.QuizQuestions.Any(e => e.QuizQuestionId == id);
        }
    }
}
