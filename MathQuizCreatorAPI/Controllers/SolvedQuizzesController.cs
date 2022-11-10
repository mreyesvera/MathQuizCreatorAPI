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
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace MathQuizCreatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SolvedQuizzesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SolvedQuizzesController(AppDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/SolvedQuizzes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SolvedQuizSimplifiedDto>>> GetSolvedQuizzes(Guid? quizId)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Guid guidUserId;

            if (userId == null || !Guid.TryParse(userId, out guidUserId))
            {
                return BadRequest("Unidentified user.");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                return BadRequest("Unidentified user.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            if(roles == null || roles.Count != 1)
            {
                return BadRequest("Invalid user.");
            }

            var role = roles[0];

            Expression<Func<SolvedQuiz, bool>> whereClause;
            if(role == "Creator")
            {
                whereClause = (solvedQuiz) => solvedQuiz.Quiz.Creator.Id == guidUserId;
            } else if (role == "Learner")
            {
                whereClause = (solvedQuiz) => solvedQuiz.UserId == guidUserId;
            } else
            {
                return BadRequest("Invalid user role.");
            }

            var solvedQuizzes = await _context.SolvedQuizzes
                .Include(solvedQuiz => solvedQuiz.Quiz)
                .ThenInclude(quiz => quiz.Creator)
                .Where(solvedQuiz => (quizId == null || solvedQuiz.QuizId == quizId))
                .Where(whereClause)
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
