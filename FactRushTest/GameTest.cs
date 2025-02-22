using Bunit;
using FactRush.Components;
using FactRush.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using FactRush.Models;
using FactRushTest.Services;

namespace FactRushTest
{
    // This class tests the game behavior by rendering the GameInterface component.
    public class GameTest : TestContext, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly FakeHttpMessageHandler _fakeHandler;
        private readonly GameState _gameState;
        private readonly Mock<TopScoreService> _mockTopScoreService;

        public GameTest()
        {
            // Setup a fake HTTP handler to intercept API calls.
            _fakeHandler = new FakeHttpMessageHandler();
            _httpClient = new HttpClient(_fakeHandler);
            Services.AddSingleton<HttpClient>(_ => _httpClient);
            // Register the IQuestionService with the configured HttpClient
            Services.AddScoped<IQuestionService, QuestionService>(sp => new QuestionService(_httpClient));

            // Provide a real GameState instance.
            _gameState = new GameState();
            Services.AddSingleton(_gameState);

            // Use a mock for the TopScoreService.
            _mockTopScoreService = new Mock<TopScoreService>();
            Services.AddSingleton(_mockTopScoreService.Object);
        }

        [Fact]
        public async Task GameInterface_Should_LoadInitialQuestion()
        {
            // Utilise le HttpClient simulé
            var questionService = Services.GetRequiredService<IQuestionService>();

            // Remplace l'appel réel par le mocké
            Question[] questions = await questionService.LoadQuestions(1);

            Assert.NotNull(questions);
            Assert.Single(questions);
            Assert.Contains("Mock Question", questions[0].Text);
        }


        [Fact]
        public void GameInterface_Should_IncreaseScore_OnCorrectAnswer()
        {
            // Utilise le HttpClient simulé
            var questionService = Services.GetRequiredService<IQuestionService>();

            // Arrange: Render the component.
            var component = RenderComponent<GameInterface>();

            // Act: Simulate a click on the correct answer ("Paris").
            var correctButton = component.FindAll("button").First(btn => btn.TextContent.Contains("Correct Answer"));
            correctButton.Click();

            // Assert: The score should increase by 100 (since the difficulty is "easy").
            component.WaitForAssertion(() =>
            {
                Assert.Contains("Score : 100", component.Markup);
            }, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void GameInterface_Should_EndGame_OnWrongAnswer()
        {
            // Arrange: Render the component.
            var questionService = Services.GetRequiredService<IQuestionService>();
            var gameComponent = RenderComponent<FactRush.Pages.Game>();
            var gameIComponent = RenderComponent<GameInterface>();

            // Act: Click on a wrong answer ("London").
            var wrongButton = gameIComponent.FindAll("button").First(btn => btn.TextContent.Contains("Wrong Answer"));
            wrongButton.Click();

            // Assert: The game over message should appear.
            gameComponent.WaitForAssertion(() =>
            {
                Assert.Contains("Game Over!", gameComponent.Markup);
            }, TimeSpan.FromSeconds(5));
            
        }

        public new void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
