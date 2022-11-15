using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MathQuizCreatorAPI.Data;
using MathQuizCreatorAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MathQuizCreatorAPI.Controllers
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// This controller manages actions for Parmaeters.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ParametersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ParametersController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        /**
         * Started working on this method, but may not need it
         */ 
        // GET: api/Parameters
        [HttpGet]
        //[Authorize(Roles = "Creator")]
        public async Task<ActionResult<IEnumerable<Parameter>>> GetParameters(Guid questionId)
        {
            //if (_httpContextAccessor.HttpContext == null
            //    || _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == null)
            //{
            //    return Unauthorized();
            //}

            //var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //Guid guidUserId;

            //if (userId == null || !Guid.TryParse(userId, out guidUserId))
            //{
            //    return BadRequest("Unidentified user.");
            //}

            //if(questionId == null)
            //{
            //    return BadRequest("Question Id can't be empty.");
            //}

            //var parameters = await _context.Parameters
            //                    .Include(param => param.Question)
            //                    .ThenInclude(question => question.Creator)
            //                    .Where(param => param.QuestionId == questionId)
            //                    .Where(param => param.Question.Creator.Id == guidUserId)
            //                    .ToListAsync();


            //return parameters;

            return await _context.Parameters.ToListAsync();
        }

        // GET: api/Parameters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Parameter>> GetParameter(Guid id)
        {
            var parameter = await _context.Parameters.FindAsync(id);

            if (parameter == null)
            {
                return NotFound();
            }

            return parameter;
        }

        // PUT: api/Parameters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParameter(Guid id, Parameter parameter)
        {
            if (id != parameter.ParameterId)
            {
                return BadRequest();
            }

            _context.Entry(parameter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParameterExists(id))
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

        // POST: api/Parameters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Parameter>> PostParameter(Parameter parameter)
        {
            _context.Parameters.Add(parameter);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParameter", new { id = parameter.ParameterId }, parameter);
        }

        // DELETE: api/Parameters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParameter(Guid id)
        {
            var parameter = await _context.Parameters.FindAsync(id);
            if (parameter == null)
            {
                return NotFound();
            }

            _context.Parameters.Remove(parameter);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParameterExists(Guid id)
        {
            return _context.Parameters.Any(e => e.ParameterId == id);
        }
    }
}
