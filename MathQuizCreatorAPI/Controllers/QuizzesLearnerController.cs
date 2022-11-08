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
using MathQuizCreatorAPI.DTOs.Quiz;
using MathQuizCreatorAPI.DTOs.QuizQuestion;
using MathQuizCreatorAPI.DTOs.Topic;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MathQuizCreatorAPI.DTOs.SolvedQuiz;

namespace MathQuizCreatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuizzesLearnerController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QuizzesLearnerController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/QuizzesLearner
        /*[HttpGet]
        public async Task<ActionResult<IEnumerable<Quiz>>> GetQuizzes()
        {
            return await _context.Quizzes.ToListAsync();
        }*/

        private async Task<QuestionSimplifiedSafeDto> GetQuestionSimplified(Guid? questionId)
        {
            var question = await _context.Questions.Where(question => question.QuestionId == questionId).FirstOrDefaultAsync();

            if (questionId == null)
            {
                return null;
            }

            var questionSimplified = new QuestionSimplifiedSafeDto()
            {
                QuestionId = questionId ?? Guid.Empty,
                Title = question.Title,
                Description = question.Description,
                AssignedQuizzes = await QuestionsController.GetAssignedQuizzes(_context, questionId ?? Guid.Empty)
            };

            return questionSimplified;
        }

        private async Task<List<QuizQuestionQuestionDeepSafeDto>> GetQuizQuestionsQuestionDeep(Guid quizQuestionId)
        {
            var quizQuestions = await _context.QuizQuestions.Where(quizQuestion => quizQuestion.QuizId == quizQuestionId).ToListAsync();

            var quizQuestionsQuestions = new List<QuizQuestionQuestionDeepSafeDto>();

            foreach (var quizQuestion in quizQuestions)
            {
                quizQuestionsQuestions.Add(new QuizQuestionQuestionDeepSafeDto()
                {
                    QuizQuestionId = quizQuestion.QuizQuestionId,
                    QuizId = quizQuestion.QuizId,
                    QuestionId = quizQuestion.QuestionId,
                    Question = await GetQuestionSimplified(quizQuestion.QuestionId),
                    Order = quizQuestion.Order
                });
            }

            return quizQuestionsQuestions;
        }

        // GET: api/QuizzesLearner/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuizDeepSafeDto>> GetQuiz(Guid id)
        {
            var quiz = await _context.Quizzes
                .Where(quiz => quiz.QuizId == id)
                .Include(quiz => quiz.Topic)
                .Include(quiz => quiz.QuizQuestions)
                .Include(quiz => quiz.Creator)
                .FirstOrDefaultAsync();

            if (quiz == null)
            {
                return NotFound();
            }

            var quizDeep = new QuizDeepSafeDto()
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
                    UserId = quiz.Creator.Id,
                    Email = quiz.Creator.Email,
                    UserName = quiz.Creator.UserName,
                },
                QuizQuestions = await GetQuizQuestionsQuestionDeep(quiz.QuizId)

            };


            return quizDeep;
        }

        // Adapt to parametrization
        private static string GetParametrizedAnswer(string correctAnswer, List<Parameter> parameters)
        {
            if(parameters != null && parameters.Count > 0)
            {
                // DO PARAMETER LOGIC HERE
            }

            return correctAnswer;
        }


        // Will need to adapt to parametrization
        private async Task<AnsweredQuestionGraded> GradeQuestion(AnsweredQuestion answeredQuestion, Question question)
        {
            var parameters = await _context.Parameters.Where(p => p.QuestionId == question.QuestionId).ToListAsync();

            var correctAnswer = GetParametrizedAnswer(question.Answer, parameters);

            var correct = answeredQuestion.Answer == correctAnswer;

            return new AnsweredQuestionGraded()
            {
                QuestionId = question.QuestionId,
                Answer = answeredQuestion.Answer,
                Correct = correct,
                CorrectAnswer = correctAnswer,
            };
        } 

        [HttpPost]
        public async Task<ActionResult<AnsweredQuizGraded>> PostQuiz(AnsweredQuiz answeredQuiz)
        {
            try
            {

                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Guid guidUserId;

                if (userId == null || !Guid.TryParse(userId, out guidUserId))
                {
                    throw new ArgumentException("Unidentified user.");
                }

                var answeredQuestions = answeredQuiz.AnsweredQuestions;

                // when trying to solve a quiz there should be answers to the questions, can't grade empty quiz
                if(answeredQuestions == null || answeredQuestions.Count == 0)
                {
                    throw new ArgumentException("Can't have empty answered questions.");
                }

                var quizQuestions = await _context.QuizQuestions
                    .Where(qq => qq.QuizId == answeredQuiz.QuizId)
                    .Include(qq => qq.Question)
                    .ToListAsync();

                if(quizQuestions == null || quizQuestions.Count == 0)
                {
                    throw new ArgumentException("Can't grade quiz without questions.");
                }

                if (quizQuestions.Count != answeredQuiz.AnsweredQuestions.Count)
                {
                    throw new ArgumentException("Mismatch of quiz's questions and sent answered questions.");
                }


                var answeredQuestionsGraded = new List<AnsweredQuestionGraded>();

                var correctResponses = 0;
                var incorrectResponses = 0;

                foreach (var quizQuestion in quizQuestions)
                {
                    var question = quizQuestion.Question;

                    if (question == null)
                    {
                        throw new ArgumentException("Question was not found.");
                    }

                    var foundAnsweredQuestions = answeredQuestions.Where(aq => aq.QuestionId == question.QuestionId).ToList();

                    if (foundAnsweredQuestions == null || foundAnsweredQuestions.Count != 1)
                    {
                        throw new ArgumentException("Mismatch of quiz's questions and sent answered questions.");
                    }

                    var answeredQuestion = foundAnsweredQuestions[0];

                    var answeredQuestionGraded = await GradeQuestion(answeredQuestion, question);
                    answeredQuestionsGraded.Add(answeredQuestionGraded);

                    if(answeredQuestionGraded.Correct == true)
                    {
                        correctResponses++;
                    } else
                    {
                        incorrectResponses++;
                    }
                }

                var totalQuestions = correctResponses + incorrectResponses;
                if(totalQuestions != quizQuestions.Count)
                {
                    throw new InvalidOperationException("Logic error while grading test.");
                }

                var solvedQuiz = new SolvedQuiz()
                {
                    UserId = guidUserId,
                    QuizId = answeredQuiz.QuizId,
                    CorrectResponses = correctResponses,
                    IncorrectResponses = incorrectResponses
                };

                _context.SolvedQuizzes.Add(solvedQuiz);
                await _context.SaveChangesAsync();


                var answeredQuizGraded = new AnsweredQuizGraded()
                {
                    QuizId = answeredQuiz.QuizId,
                    AnsweredQuestionsGraded = answeredQuestionsGraded,
                    SolvedQuiz = new SolvedQuizSimplifiedDto()
                    {
                        SolvedQuizId = solvedQuiz.SolvedQuizId,
                        CorrectResponses = solvedQuiz.CorrectResponses,
                        IncorrectResponses = solvedQuiz.IncorrectResponses,
                        TotalQuestions = solvedQuiz.TotalQuestions,
                        Score = solvedQuiz.Score
                    }
                };

                return answeredQuizGraded;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }
    }
}
