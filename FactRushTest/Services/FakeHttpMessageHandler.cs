using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace FactRushTest.Services
{
    /// <summary>
    /// A fake HTTP message handler to mock API responses for testing purposes.
    /// </summary>
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private static readonly string[] item = ["Wrong Answer 1", "Wrong Answer 2", "Wrong Answer 3"];
        /// <summary>
        /// Intercepts HTTP requests and returns predefined responses based on the request URL.
        /// </summary>
        /// <param name="request">The incoming HTTP request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A predefined HTTP response based on the request.</returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Intercepted request: {request.RequestUri}");

            // Mock token request response
            if (request.RequestUri.AbsoluteUri.Contains("api_token.php"))
            {
                Debug.WriteLine("Returning dummy token...");
                var json = JsonSerializer.Serialize(new { response_code = 0, token = "dummy_tocken" });
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json)
                });
            }
            // Mock trivia questions request response
            else if (request.RequestUri.AbsoluteUri.Contains("api.php"))
            {
                Debug.WriteLine($"Request parameters: {request.RequestUri.Query}");

                // Extract the requested number of questions and limit it to a maximum of 10
                int amount = Math.Min(ExtractAmountFromQuery(request.RequestUri.Query), 10);

                // Generate a dynamic list of questions
                var questions = GenerateMockQuestions(amount);
                var jsonResponse = JsonSerializer.Serialize(new { response_code = 0, results = questions });

                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(jsonResponse)
                });
            }

            Debug.WriteLine("Returning 404 Not Found...");
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        }

        /// <summary>
        /// Extracts the requested number of questions from the query string.
        /// Defaults to 1 if parsing fails.
        /// </summary>
        /// <param name="query">The query string from the request URL.</param>
        /// <returns>The number of requested questions, defaulting to 1 if invalid.</returns>
        private static int ExtractAmountFromQuery(string query)
        {
            var queryParams = System.Web.HttpUtility.ParseQueryString(query);
            if (int.TryParse(queryParams["amount"], out int amount) && amount > 0)
            {
                return amount;
            }
            return 1; // Default to 1 if no valid amount is found
        }

        /// <summary>
        /// Generates a list of mock trivia questions.
        /// </summary>
        /// <param name="count">The number of questions to generate.</param>
        /// <returns>A list of mock questions.</returns>
        private static List<object> GenerateMockQuestions(int count)
        {
            var questions = new List<object>();
            for (int i = 1; i <= count; i++)
            {
                questions.Add(new
                {
                    category = "General Knowledge",
                    type = "multiple",
                    difficulty = "easy",
                    question = $"Mock Question {i}?",
                    correct_answer = "Correct Answer",
                    incorrect_answers = item
                });
            }
            return questions;
        }
    }
}
