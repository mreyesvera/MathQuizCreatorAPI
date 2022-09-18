using Microsoft.EntityFrameworkCore;

namespace MathQuizCreatorAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
