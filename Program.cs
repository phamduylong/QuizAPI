Random rand = new Random();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at
// https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

var quizzes = new List<Quiz> {  };

quizzes.Add(new Quiz("Who created Svelte?",
    new List<string> { "Rich Harris", "Linus Torsvald", 
        "Mark Zuckerberg", "Ada Lovelace" }, "Rich Harris"));

app.MapGet("/quiz", () =>
{
    return new Quiz(quizzes[rand.Next(quizzes.Count)]);
})
.WithName("GetRandomQuiz");

app.Run();

internal record Quiz(string? quiz, List<string> options, string ans)
{
    public Quiz(Quiz _quiz) {
        quiz = _quiz.quiz;
        options = _quiz.options;
        ans = _quiz.ans;
    }
}