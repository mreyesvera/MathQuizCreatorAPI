using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MathQuizCreatorAPI.Data;
using MathQuizCreatorAPI.Models;
using System.Drawing;
using MathQuizCreatorAPI.DTOs;

namespace MathQuizCreatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizzesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QuizzesController(AppDbContext context)
        {
            _context = context;
        }

        private async Task<string> GetAssignedQuizzes(Guid questionId)
        {
            var quizQuestions = await _context.QuizQuestions.Where(quizQuestion => quizQuestion.QuestionId == questionId).ToListAsync();
            string assignedQuizzes = "";

            foreach(var quizQuestion in quizQuestions)
            {
                assignedQuizzes += $"{quizQuestion.Quiz.Title}\n";
            }

            return assignedQuizzes;
        }

        private async Task<QuestionSimplifiedDto> GetQuestionSimplified(Guid? questionId)
        {
            var question = await _context.Questions.Where(question => question.QuestionId == questionId).FirstOrDefaultAsync();

            if(questionId == null)
            {
                return null;
            }

            var questionSimplified = new QuestionSimplifiedDto()
            {
                QuestionId = questionId ?? Guid.Empty,
                Title = question.Title,
                Description = question.Description,
                AssignedQuizzes = await GetAssignedQuizzes(questionId ?? Guid.Empty)
            };

            return questionSimplified;
        }

        // GET: api/Quizzes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quiz>>> GetQuizzes()
        {
            return await _context.Quizzes.ToListAsync();
        }

        private async Task<List<QuizQuestionQuestionDeepDto>> GetQuizQuestionsQuestionDeep(Guid quizQuestionId)
        {
            var quizQuestions = await _context.QuizQuestions.Where(quizQuestion => quizQuestion.QuizId == quizQuestionId).ToListAsync();

            var quizQuestionsQuestions = new List<QuizQuestionQuestionDeepDto>();

            foreach(var quizQuestion in quizQuestions)
            {
                quizQuestionsQuestions.Add(new QuizQuestionQuestionDeepDto()
                {
                    QuizQuestionId = quizQuestion.QuizQuestionId,
                    QuestionId = quizQuestion.QuestionId,
                    Question = await GetQuestionSimplified(quizQuestion.QuestionId),
                    Order = quizQuestion.Order
                });
            }

            return quizQuestionsQuestions;
        }

        // GET: api/Quizzes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuizDeepDto>> GetQuiz(Guid id)
        {
            var quiz = await _context.Quizzes
                .Where(quiz => quiz.QuizId == id)
                .Include(quiz => quiz.Topic)
                .Include(quiz => quiz.QuizQuestions)
                .Include(quiz => quiz.Creator)
                .Include(quiz => quiz.Creator.Role)
                .FirstOrDefaultAsync();

            if (quiz == null)
            {
                return NotFound();
            }

            var quizDeep = new QuizDeepDto()
            {
                QuizId = quiz.QuizId,
                Title = quiz.Title,
                Description = quiz.Description,
                IsPublic = quiz.IsPublic,
                HasUnlimitedMode = quiz.HasUnlimitedMode,
                LastModifiedTime = quiz.LastModifiedTime,
                CreationTime = quiz.CreationTime,
                Topic = new TopicSimplifiedDto()
                {
                    TopicId = quiz.Topic.TopicId,
                    Title = quiz.Topic.Title
                },
                Creator = new UserSimplifiedDto()
                {
                    UserId = quiz.Creator.UserId,
                    Email = quiz.Creator.Email,
                    Username = quiz.Creator.Username,
                    Role = new RoleSimplifiedDto()
                    {
                        RoleId = quiz.Creator.Role.RoleId,
                        Title = quiz.Creator.Role.Title
                    },
                },
                QuizQuestions = await GetQuizQuestionsQuestionDeep(quiz.QuizId)

            };


            return quizDeep;
        }

        // PUT: api/Quizzes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuiz(Guid id, QuizEditDto quizEdit)
        {
            if (id != quizEdit.QuizId)
            {
                return BadRequest();
            }

            var quiz = await _context.Quizzes.Where(quiz => quiz.QuizId == id).FirstOrDefaultAsync();

            if (quiz == null)
            {
                return NotFound();
            }

            quiz.Title = quizEdit.Title;
            quiz.Description = quizEdit.Description;
            quiz.IsPublic = quizEdit.IsPublic;
            quiz.HasUnlimitedMode = quizEdit.HasUnlimitedMode;

            _context.Entry(quiz).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizExists(id))
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

        // POST: api/Quizzes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<QuizSimplifiedDto>> PostQuiz(QuizAddDto quizAdd)
        {
            Guid? topicId = quizAdd.TopicId;

            if (topicId == null)
            {
                throw new ArgumentException("Topic id can't be empty.");
            }

            Topic topic = await _context.Topics.Where(topic => topic.TopicId == topicId).SingleOrDefaultAsync();

            if (topic == null)
            {
                throw new ArgumentException("Topic couldn't be found with the given Topic Id.");
            }

            Guid? creatorId = quizAdd.CreatorId;

            if (creatorId == null)
            {
                throw new ArgumentException("Creatr id can't be empty.");
            }

            User creator = await _context.Users.Where(creator => creator.UserId == creatorId).SingleOrDefaultAsync();

            if (creator == null)
            {
                throw new ArgumentException("Topic couldn't be found with the given Topic Id.");
            }

            var quiz = new Quiz()
            {
                Title = quizAdd.Title,
                Description = quizAdd.Description,
                IsPublic = quizAdd.IsPublic,
                HasUnlimitedMode = quizAdd.HasUnlimitedMode,
                Topic = topic,
                Creator = creator,
            };

            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            var quizSimplified = new QuizSimplifiedDto()
            {
                QuizId = quiz.QuizId,
                Title = quiz.Title,
                Description = quiz.Description,
                IsPublic = quiz.IsPublic,
                HasUnlimitedMode = quiz.HasUnlimitedMode,
            };

            return CreatedAtAction("GetQuiz", new { id = quiz.QuizId }, quizSimplified);
        }

        // DELETE: api/Quizzes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(Guid id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuizExists(Guid id)
        {
            return _context.Quizzes.Any(e => e.QuizId == id);
        }
    }
}
