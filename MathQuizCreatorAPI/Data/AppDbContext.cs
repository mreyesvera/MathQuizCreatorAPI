using MathQuizCreatorAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MathQuizCreatorAPI.Data
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Application Database Context.  
    /// </summary>
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid> 
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Topic>? Topics { get; set; }
        public DbSet<Quiz>? Quizzes { get; set; }
        public DbSet<SolvedQuiz>? SolvedQuizzes { get; set; }
        public DbSet<Question>? Questions { get; set; }
        public DbSet<QuizQuestion>? QuizQuestions { get; set; }
        public DbSet<NormalDistribution>? NormalDistributions { get; set; }
        public DbSet<Parameter>? Parameters { get; set; }


    }
}
