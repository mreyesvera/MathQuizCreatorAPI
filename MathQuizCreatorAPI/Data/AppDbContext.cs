using MathQuizCreatorAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MathQuizCreatorAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User>? Users { get; set; }
        public DbSet<Role>? Roles { get; set; }
        public DbSet<Topic>? Topics { get; set; }
        public DbSet<Quiz>? Quizzes { get; set; }
        public DbSet<SolvedQuiz>? SolvedQuizzes { get; set; }
        public DbSet<Question>? Questions { get; set; }
        public DbSet<QuizQuestion>? QuizQuestions { get; set; }
        public DbSet<NormalDistribution>? NormalDistributions { get; set; }
        public DbSet<Parameter>? Parameters { get; set; }

    }
}
