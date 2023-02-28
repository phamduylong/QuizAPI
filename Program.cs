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

app.MapPost("/quizzes", async (QuizDb db, Quiz quiz) => {
    db.Quizzes.Add(quiz);
    await db.SaveChangesAsync();
    return Results.Created($"/quizzes/{quiz.Id}", quiz);
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



