using System.Net;
using System.Text.Json.Serialization;

namespace FactRush.Components
{
    public class Question
    {
        [JsonPropertyName("category")]
        public string Category { get; set; } = "";
        [JsonPropertyName("type")]
        public string Type { get; set; } = "";
        [JsonPropertyName("difficulty")]
        public string Difficulty { get; set; } = "";
        [JsonPropertyName("question")]
        public string Text { get; set; } = "";
        [JsonPropertyName("correct_answer")]
        public string CorrectAnswer { get; set; } = "";
        [JsonPropertyName("incorrect_answers")]
        public string[] IncorrectAnswers { get; set; } = [];

        public void DecodeHtmlEntities()
        {
            Text = WebUtility.HtmlDecode(Text);
            CorrectAnswer = WebUtility.HtmlDecode(CorrectAnswer);
            for (int i = 0; i < IncorrectAnswers.Length; i++)
            {
                IncorrectAnswers[i] = WebUtility.HtmlDecode(IncorrectAnswers[i]);
            }
        }

        public override string ToString()
        {
            return $"Text: \"{Text}\", CorrectAnswer: \"{CorrectAnswer}\", Difficulty: \"{Difficulty}\"";
        }
    }
}
