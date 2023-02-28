using Newtonsoft.Json;
public class Quiz
{
    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("answer")]
    public string Answer { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

}