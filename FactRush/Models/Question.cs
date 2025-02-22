﻿using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace FactRush.Models
{
    /// <summary>
    /// Represents a quiz question fetched from the API.
    /// </summary>
    public class Question
    {
        /// <summary>
        /// The category of the question.
        /// </summary>
        [JsonPropertyName("category")]
        public string Category { get; set; } = "";
        /// <summary>
        /// The type of the question (e.g., "boolean").
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = "";
        /// <summary>
        /// The difficulty level of the question (e.g., "easy", "medium", "hard").
        /// </summary>
        [JsonPropertyName("difficulty")]
        public string Difficulty { get; set; } = "";
        /// <summary>
        /// The text of the question.
        /// </summary>
        [JsonPropertyName("question")]
        public string Text { get; set; } = "";
        /// <summary>
        /// The correct answer for the question.
        /// </summary>
        [JsonPropertyName("correct_answer")]
        public string CorrectAnswer { get; set; } = "";
        /// <summary>
        /// The incorrect answers for the question.
        /// </summary>
        [JsonPropertyName("incorrect_answers")]
        public string[] IncorrectAnswers { get; set; } = [];

        /// <summary>
        /// Decodes HTML entities in the question text and answers.
        /// </summary>
        public void DecodeHtmlEntities()
        {
            Text = WebUtility.HtmlDecode(Text);
            CorrectAnswer = WebUtility.HtmlDecode(CorrectAnswer);
            IncorrectAnswers = Array.ConvertAll(IncorrectAnswers, x => WebUtility.HtmlDecode(x)!);
        }

        /// <summary>
        /// Returns a string representation of the question.
        /// </summary>
        /// <returns>A formatted string containing the question and its details.</returns>
        public override string ToString() => $"Text: \"{Text}\", CorrectAnswer: \"{CorrectAnswer}\", Difficulty: \"{Difficulty}\"";

        /// <summary>
        /// Load a given number of questions.
        /// </summary>
        /// <param name="amount">The number of question to load. Should be greater than 0.</param>
        /// <param name="token" optional="true">The optionnal token to use</param>
        /// <returns>A list of Question object.</returns>
        public static async Task<Question[]> LoadQuestions(int amount, string token = "")
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than 0.");
            }
            Console.WriteLine($"Fetching {amount} more questions");
            string url = $"https://opentdb.com/api.php?amount={amount}&token={token}";
            var result = await new HttpClient().GetFromJsonAsync<QuestionResponse>(url);
            if (result != null && result.ResponseCode == 0 && result.Questions.Length == amount)
            {   
                foreach (var q in result.Questions)
                {
                    q.DecodeHtmlEntities();
                }
                return result.Questions;
            }
            else
            {
                Console.WriteLine("Retrying...");
                await Task.Delay(5000);
                return await LoadQuestions(amount, token);
            }
        }

        /// <summary>
        /// Generates list of the answer's choices.
        /// </summary>
        /// <param name="question">The Question object from which to load the choice.</param>
        /// <returns>A list of string containing answer's choice.</returns>
        public static List<string> GenerateAnswerChoices(Question question)
        {
            if (question.Type == "boolean")
            {
                return ["True", "False"];
            }
            else if (question.Type == "multiple")
            {
                return [.. question.IncorrectAnswers
                    .Concat([question.CorrectAnswer])
                    .OrderBy(x => Guid.NewGuid())];
            }
            else
            {
                return [question.CorrectAnswer];
            }
        }
    }
}
