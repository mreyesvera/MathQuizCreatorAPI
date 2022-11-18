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
using MathQuizCreatorAPI.DTOs.Parameter;

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
    [Authorize(Roles = "Creator")]
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
        /*[HttpGet]
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
        }*/

        /// <summary>
        /// Returns a parameter created by the user based on the provided id. 
        /// </summary>
        /// <param name="id">Parameter id to look for</param>
        /// <returns>found parameter</returns>
        // GET: api/Parameters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ParameterSimplifiedDto>> GetParameter(Guid id)
        {
            try
            {
                var parameter = await _context.Parameters
                    .Where(parameter => parameter.ParameterId == id)
                    .Include(parameter => parameter.Question)
                    .ThenInclude(question => question.Creator)
                    .FirstOrDefaultAsync();

                if (parameter == null)
                {
                    return NotFound();
                }

                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Guid guidUserId;

                if (userId == null || !Guid.TryParse(userId, out guidUserId))
                {
                    return BadRequest("Unidentified user.");
                }

                if (parameter.Question.Creator.Id != guidUserId)
                {
                    return Unauthorized();
                }

                if (parameter == null)
                {
                    return NotFound();
                }

                var parameterSimplified = new ParameterSimplifiedDto()
                {
                    ParameterId = parameter.ParameterId,
                    Name = parameter.Name,
                    Value = parameter.Value,
                    Order = parameter.Order,
                    QuestionId = parameter.QuestionId
                };

                return parameterSimplified;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }

        /// <summary>
        /// Updates a parameter from the provided id, with the editted parameter data. 
        /// </summary>
        /// <param name="id">parameter id of parameter to update</param>
        /// <param name="parameterUpdated">parameter updated</param>
        /// <returns>no content if succeeded or an error otherwise</returns>
        // PUT: api/Parameters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParameter(Guid id, Parameter parameterUpdated)
        {
            try
            {
                if (id != parameterUpdated.ParameterId)
                {
                    return BadRequest();
                }

                var parameter = await _context.Parameters
                    .Where(parameter => parameter.ParameterId == id)
                    .Include(parameter => parameter.Question)
                    .ThenInclude(question => question.Creator)
                    .FirstOrDefaultAsync();

                if (parameter == null || parameter.Question == null)
                {
                    return NotFound();
                }

                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Guid guidUserId;

                if (userId == null || !Guid.TryParse(userId, out guidUserId))
                {
                    return BadRequest("Unidentified user.");
                }

                if (parameter.Question.Creator.Id != guidUserId)
                {
                    return Unauthorized();
                }

                if(parameter.QuestionId != parameterUpdated.QuestionId)
                {
                    return BadRequest("Can't change question id.");
                }

                if(parameter.Name != parameterUpdated.Name)
                {
                    return BadRequest("Can't change parameter name.");
                }

                parameter.Value = parameterUpdated.Value;
                parameter.Order = parameterUpdated.Order;

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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }

        /// <summary>
        /// Creates a new parameter for the user given the provided added parameter.
        /// </summary>
        /// <param name="parameterAdd">added parameter data</param>
        /// <returns>Created parameter</returns>
        // POST: api/Parameters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ParameterSimplifiedDto>> PostParameter(ParameterAddDto parameterAdd)
        {
            try
            {
                Guid? questionId = parameterAdd.QuestionId;

                if (questionId == null)
                {
                    return BadRequest("Question id can't be empty.");
                }

                var question = await _context.Questions
                    .Include(question => question.Creator)
                    .Where(question => question.QuestionId == questionId)
                    .SingleOrDefaultAsync();

                if (question == null)
                {
                    return BadRequest("Question couldn't be found with the given Question Id.");
                }

                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Guid creatorId;

                if (userId == null || !Guid.TryParse(userId, out creatorId))
                {
                    return BadRequest("Unidentified user.");
                }

                ApplicationUser creator = await _context.Users.Where(creator => creator.Id == creatorId).SingleOrDefaultAsync();

                if (creator == null || question.Creator.Id != creatorId)
                {
                    return Unauthorized();
                }

                var parameter = new Parameter()
                {
                    Name = parameterAdd.Name,
                    Value = parameterAdd.Value,
                    Order = parameterAdd.Order,
                    QuestionId = parameterAdd.QuestionId
                };

                _context.Parameters.Add(parameter);
                await _context.SaveChangesAsync();

                var parameterSimplified = new ParameterSimplifiedDto()
                {
                    ParameterId = parameter.ParameterId,
                    Name = parameter.Name,
                    Value = parameter.Value,
                    Order = parameter.Order,
                    QuestionId = parameter.QuestionId
                };

                return CreatedAtAction("GetParameter", new { id = parameter.ParameterId }, parameterSimplified);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }

        /// <summary>
        /// Deletes a parameter with the provided id.
        /// </summary>
        /// <param name="id">parameter id of parameter to delete</param>
        /// <returns>no content if succeeded or error otherwise</returns
        // DELETE: api/Parameters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParameter(Guid id)
        {
            try
            {
                var parameter = await _context.Parameters
                    .Where(parameter => parameter.ParameterId == id)
                    .Include(parameter => parameter.Question)
                    .ThenInclude(question => question.Creator)
                    .FirstOrDefaultAsync();

                if (parameter == null)
                {
                    return NotFound();
                }

                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Guid guidUserId;

                if (userId == null || !Guid.TryParse(userId, out guidUserId))
                {
                    return BadRequest("Unidentified user.");
                }

                if (parameter.Question.Creator.Id != guidUserId)
                {
                    return Unauthorized();
                }

                _context.Parameters.Remove(parameter);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }

        /// <summary>
        /// Returns whether a parameter exists based on a provided id.
        /// </summary>
        /// <param name="id">parameter id of parameter to look for</param>
        /// <returns>true if parameter is found, false otherwise</returns>
        private bool ParameterExists(Guid id)
        {
            return _context.Parameters.Any(e => e.ParameterId == id);
        }
    }
}
