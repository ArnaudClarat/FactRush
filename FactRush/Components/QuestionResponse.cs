using System.Text;
using System.Text.Json.Serialization;

namespace FactRush.Components
{
    public class QuestionResponse
    {
        [JsonPropertyName("response_code")]
        public int ResponseCode { get; set; }
        [JsonPropertyName("results")]
        public Question[] Questions { get; set; } = [];

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"ResponseCode: {ResponseCode}");

            if (Questions.Length > 0)
            {
                sb.AppendLine("Questions:");
                foreach (var question in Questions)
                {
                    sb.AppendLine($"- {question}");
                }
            }
            else
            {
                sb.AppendLine("No questions available.");
            }

            return sb.ToString();
        }
    }
}
