using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MathQuizCreatorAPI.Data;
using MathQuizCreatorAPI.Models;
using MathQuizCreatorAPI.DTOs.SolvedQuiz;
using Microsoft.AspNetCore.Authorization;

namespace MathQuizCreatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SolvedQuizzesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SolvedQuizzesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/SolvedQuizzes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SolvedQuizSimplifiedDto>>> GetSolvedQuizzes(Guid? quizId)
        {
            var solvedQuizzes = await _context.SolvedQuizzes
                .Where(solvedQuiz => (quizId == null || solvedQuiz.QuizId == quizId))
                .ToListAsync();

            var solvedQuizzesSimplified = new List<SolvedQuizSimplifiedDto>();

            foreach(var solvedQuiz in solvedQuizzes)
            {
                solvedQuizzesSimplified.Add(new SolvedQuizSimplifiedDto()
                {
                    SolvedQuizId = solvedQuiz.SolvedQuizId,
                    CorrectResponses = solvedQuiz.CorrectResponses,
                    IncorrectResponses = solvedQuiz.IncorrectResponses,
                    TotalQuestions = solvedQuiz.TotalQuestions,
                    Score = solvedQuiz.Score
                });
            }   

            return solvedQuizzesSimplified;
        }

        // GET: api/SolvedQuizzes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SolvedQuiz>> GetSolvedQuiz(Guid id)
        {
            var solvedQuiz = await _context.SolvedQuizzes.FindAsync(id);

            if (solvedQuiz == null)
            {
                return NotFound();
            }

            return solvedQuiz;
        }

        // PUT: api/SolvedQuizzes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSolvedQuiz(Guid id, SolvedQuiz solvedQuiz)
        {
            if (id != solvedQuiz.SolvedQuizId)
            {
                return BadRequest();
            }

            _context.Entry(solvedQuiz).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolvedQuizExists(id))
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

        // POST: api/SolvedQuizzes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SolvedQuiz>> PostSolvedQuiz(SolvedQuiz solvedQuiz)
        {
            _context.SolvedQuizzes.Add(solvedQuiz);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSolvedQuiz", new { id = solvedQuiz.SolvedQuizId }, solvedQuiz);
        }

        // DELETE: api/SolvedQuizzes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolvedQuiz(Guid id)
        {
            var solvedQuiz = await _context.SolvedQuizzes.FindAsync(id);
            if (solvedQuiz == null)
            {
                return NotFound();
            }

            _context.SolvedQuizzes.Remove(solvedQuiz);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SolvedQuizExists(Guid id)
        {
            return _context.SolvedQuizzes.Any(e => e.SolvedQuizId == id);
        }
    }
}
