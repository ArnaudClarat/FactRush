using System.Text;
using System.Text.Json.Serialization;

namespace FactRush.Components
{
    /// <summary>
    /// Represents the response from the question API.
    /// </summary>
    public class QuestionResponse
    {
        /// <summary>
        /// Gets and sets the response code from the API.
        /// </summary>
        [JsonPropertyName("response_code")]
        public int ResponseCode { get; set; }
        /// <summary>
        /// Gets and sets the array of questions returned by the API.
        /// </summary>
        [JsonPropertyName("results")]
        public Question[] Questions { get; set; } = [];

        /// <summary>
        /// Returns a string representation of the question response.
        /// </summary>
        /// <returns>A formatted string containing the response code and questions.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder()
                .AppendLine($"ResponseCode: {ResponseCode}")
                .AppendLine(Questions.Length > 0 ? "Questions:" : "No questions available.");

            foreach (var question in Questions) sb.AppendLine($"- {question}");

            return sb.ToString();
        }
    }
}
