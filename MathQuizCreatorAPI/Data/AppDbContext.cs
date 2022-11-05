using MathQuizCreatorAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MathQuizCreatorAPI.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser> /*DbContext*/
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<RefreshToken> RefreshToken { get; set; }

        //public DbSet<User>? Users { get; set; }
        //public DbSet<Role>? Roles { get; set; }
        public DbSet<Topic>? Topics { get; set; }
        public DbSet<Quiz>? Quizzes { get; set; }
        public DbSet<SolvedQuiz>? SolvedQuizzes { get; set; }
        public DbSet<Question>? Questions { get; set; }
        public DbSet<QuizQuestion>? QuizQuestions { get; set; }
        public DbSet<NormalDistribution>? NormalDistributions { get; set; }
        public DbSet<Parameter>? Parameters { get; set; }


    }
}
