using Newtonsoft.Json;
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

var quizzes = new List<Quiz> { };

var quiz1 = new Quiz("Who created Svelte?",
    new List<string> { "Rich Harris", "Linus Torsvald",
        "Mark Zuckerberg", "Ada Lovelace" }, AnswerType.str, "Rich Harris");

var quiz2 = new Quiz("What version of Svelte are we in right now?", new List<string> { "0", "1", "2", "3" }, AnswerType.num, 3);

quizzes.Add(quiz1);
quizzes.Add(quiz2);

app.MapGet("/quiz", () =>
{
    var randRes = new Quiz(quizzes[rand.Next(quizzes.Count)]);
    Console.WriteLine(randRes.ToString());
    Console.WriteLine(randRes.GetType().Name);
    var json_quiz = JsonConvert.SerializeObject(quizzes);
    return json_quiz;
})
.WithName("quiz");

app.Run();

enum AnswerType
{
    str,
    num,
    binary

}

class Quiz
{
    // JSONProperty is need for serialization of private class members
    [JsonProperty("title")]
    private string title { get; set; }

    [JsonProperty("options")]
    private List<string> options { get; set; }

    [JsonProperty("answertype")]
    private AnswerType ansType { get; set; }

    [JsonProperty("answer")]
    private object ans { get; set; }

    public Quiz()
    {
        title = "";
        options = new List<string>();
        ansType = AnswerType.str;
        ans = "";
    }

    public Quiz(string _title, List<string> _options, AnswerType _ansType, object _ans)
    {
        title = _title;
        options = _options;
        ansType = _ansType;
        ans = _ans;
    }

    public Quiz(Quiz _quiz)
    {
        title = _quiz.title;
        options = _quiz.options;
        ansType = _quiz.ansType;
        ans = _quiz.ans;
    }

    public override string ToString()
    {
        return (title + " " + "\nAnswer type: " + ansType);
    }

}