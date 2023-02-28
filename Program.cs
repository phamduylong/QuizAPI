using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
Random rand = new Random();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<QuizDb>(opt => opt.UseInMemoryDatabase("QuizList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

app.MapGet("/quizzes", async (QuizDb db) => JsonConvert.SerializeObject(await db.Quizzes.ToListAsync()));

app.MapGet("/quizzes/{id}", async (QuizDb db, int id) => JsonConvert.SerializeObject(await db.Quizzes.FindAsync(id)));

app.MapPost("/quizzes", async (QuizDb db, Quiz newQuiz) => {
    db.Quizzes.Add(newQuiz);
    await db.SaveChangesAsync();
    return Results.Created($"/quizzes/{newQuiz.Id}", newQuiz);
});

app.MapPut("/quizzes/{id}", async (QuizDb db, int id, Quiz newQuiz) => {
    if(await db.Quizzes.FindAsync(id) is Quiz quiz)
    {
        quiz.Title = newQuiz.Title;
        quiz.Answer = newQuiz.Answer;
        await db.SaveChangesAsync();
        return Results.Ok();
    }

    return Results.NotFound();
});

app.MapDelete("/quizzes/{id}", async (QuizDb db, int id) => {
    if(await db.Quizzes.FindAsync(id) is Quiz quiz)
    {
        db.Quizzes.Remove(quiz);
        await db.SaveChangesAsync();
        return Results.Ok();
    }

    return Results.NotFound();
});

app.Run();



