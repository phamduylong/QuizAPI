using Microsoft.EntityFrameworkCore;

class QuizDb : DbContext
{
    // This constructor is used by the dependency injection system.
    public QuizDb(DbContextOptions<QuizDb> options)
        : base(options) { }

    public DbSet<Quiz> Quizzes => Set<Quiz>();
}