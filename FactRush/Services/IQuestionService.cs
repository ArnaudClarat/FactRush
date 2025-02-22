using FactRush.Models;

namespace FactRush.Services
{
    public interface IQuestionService
    {
        Task<Question[]> LoadQuestions(int amount, string token = "");
    }
}