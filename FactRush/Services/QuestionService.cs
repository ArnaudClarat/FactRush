using System.Net.Http.Json;
using FactRush.Models;

namespace FactRush.Services
{
    public class QuestionService(HttpClient httpClient, ILocalStorageService localStorageService) : IQuestionService
    {
        private readonly HttpClient HttpClient = httpClient;
        private readonly ILocalStorageService LocalStorageService = localStorageService;

        public async Task<Question[]> LoadQuestions(int amount, string token = "")
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than 0.");
            }
            Console.WriteLine($"Fetching {amount} more questions");
            string url = $"https://opentdb.com/api.php?amount={amount}&token={token}";
            var result = await HttpClient.GetFromJsonAsync<QuestionResponse>(url);
            if (result != null && result.ResponseCode == 0 && result.Questions.Length == amount)
            {
                foreach (var q in result.Questions)
                {
                    q.DecodeHtmlEntities();
                    await q.SetIsFavorite(LocalStorageService);
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
    }
}