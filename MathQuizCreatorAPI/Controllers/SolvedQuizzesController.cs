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
using MathQuizCreatorAPI.DTOs.Quiz;
using MathQuizCreatorAPI.DTOs.Topic;

namespace MathQuizCreatorAPI.Controllers
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// This controller manages actions for Solved Quizzes.
    /// </summary>
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

        /// <summary>
        /// Returns a list of solved quizzes, all or only for the provided quiz id.
        /// If a creator is getting them, it gets only solved quizzes of quizzes they have created.
        /// If a learner is getting them, it only returns solved quizzes they have solved.
        /// </summary>
        /// <param name="quizId">Optional quiz id to return only solved quizzes for that quiz</param>
        /// <returns>list of solved quizzes</returns>
        // GET: api/SolvedQuizzes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SolvedQuizSimplifiedDto>>> GetSolvedQuizzes(Guid? quizId)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Guid guidUserId;

                if (userId == null || !Guid.TryParse(userId, out guidUserId))
                {
                    return BadRequest("Unidentified user.");
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return BadRequest("Unidentified user.");
                }

                var roles = await _userManager.GetRolesAsync(user);

                if (roles == null || roles.Count != 1)
                {
                    return BadRequest("Invalid user.");
                }

                var role = roles[0];

                Expression<Func<SolvedQuiz, bool>> whereClause;
                if (role == "Creator")
                {
                    whereClause = (solvedQuiz) => solvedQuiz.Quiz.Creator.Id == guidUserId;
                }
                else if (role == "Learner")
                {
                    whereClause = (solvedQuiz) => solvedQuiz.UserId == guidUserId;
                }
                else
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

                foreach (var solvedQuiz in solvedQuizzes)
                {
                    solvedQuizzesSimplified.Add(new SolvedQuizSimplifiedDto()
                    {
                        SolvedQuizId = solvedQuiz.SolvedQuizId,
                        CorrectResponses = solvedQuiz.CorrectResponses,
                        IncorrectResponses = solvedQuiz.IncorrectResponses,
                        TotalQuestions = solvedQuiz.TotalQuestions,
                        Score = solvedQuiz.Score,
                        CreationTime = solvedQuiz.CreationTime,
                        LastModifiedTime = solvedQuiz.LastModifiedTime,
                    });
                }

                return solvedQuizzesSimplified;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }

        // Not needed currently
        // GET: api/SolvedQuizzes/5
        /*[HttpGet("{id}")]
        public async Task<ActionResult<SolvedQuiz>> GetSolvedQuiz(Guid id)
        {
            var solvedQuiz = await _context.SolvedQuizzes.FindAsync(id);

            if (solvedQuiz == null)
            {
                return NotFound();
            }

            return solvedQuiz;
        }*/

        // Not needed currently
        // PUT: api/SolvedQuizzes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{id}")]
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
        }*/

        // Not needed currently
        // POST: api/SolvedQuizzes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPost]
        public async Task<ActionResult<SolvedQuiz>> PostSolvedQuiz(SolvedQuiz solvedQuiz)
        {
            _context.SolvedQuizzes.Add(solvedQuiz);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSolvedQuiz", new { id = solvedQuiz.SolvedQuizId }, solvedQuiz);
        }*/

        // Not needed currently
        // DELETE: api/SolvedQuizzes/5
        /*[HttpDelete("{id}")]
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
        }*/

        // Not needed currently
        /*private bool SolvedQuizExists(Guid id)
        {
            return _context.SolvedQuizzes.Any(e => e.SolvedQuizId == id);
        }*/
    }
}
