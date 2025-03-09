using Bunit;
using FactRush.Components;
using FactRush.Models;
using FactRush.Services;
using FactRushTest.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace FactRushTest
{

    public class GameTest : TestContext, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly FakeHttpMessageHandler _fakeHandler;
        private readonly GameState _gameState;
        private readonly Mock<ILocalStorageService> mockLocalStorageService = new();

        public GameTest()
        {
            // Setup a fake HTTP handler to intercept API calls.
            _fakeHandler = new FakeHttpMessageHandler();
            _httpClient = new HttpClient(_fakeHandler);
            Services.AddSingleton<HttpClient>(_ => _httpClient);
            // Register the IQuestionService with the configured HttpClientvar mockLocalStorageService = new Mock<ILocalStorageService>();
            mockLocalStorageService
                .Setup(s => s.GetItemAsync<List<Question>>("favorites"))
                .ReturnsAsync([]);
            Services.AddScoped<ILocalStorageService, LocalStorageService>();
            Services.AddScoped<IQuestionService, QuestionService>(sp => new QuestionService(_httpClient, mockLocalStorageService.Object));

            // Provide a real GameState instance.
            _gameState = new GameState();
            Services.AddSingleton(_gameState);

            // Register the real TopScoreService (scoped) that depends on LocalStorageService.
            Services.AddScoped<TopScoreService>();
        }

        [Fact]
        public async Task GameInterface_Should_LoadInitialQuestion()
        {
            // For this test, we use the IQuestionService to verify that the fake HTTP handler returns valid questions.
            var questionService = Services.GetRequiredService<IQuestionService>();
            Question[] questions = await questionService.LoadQuestions(1);

            Assert.NotNull(questions);
            Assert.Single(questions);
            Assert.Contains("Mock Question", questions[0].Text);
        }

        [Fact]
        public void GameInterface_Should_IncreaseScore_OnCorrectAnswer()
        {
            // Arrange: Render the GameInterface component.
            var component = RenderComponent<GameInterface>();

            // Wait for the answers to be enabled (they are activated after ~5 seconds).
            component.WaitForAssertion(() =>
            {
                var buttons = component.FindAll("button");
                Assert.Contains(buttons, btn => btn.TextContent.Contains("Correct Answer") && !btn.HasAttribute("disabled"));
            }, TimeSpan.FromSeconds(7));

            // Act: Simulate a click on the correct answer.
            var correctButton = component.FindAll("button").First(btn => btn.TextContent.Contains("Correct Answer"));
            correctButton.Click();

            // Assert: The score should increase by 100 (assuming difficulty "easy").
            component.WaitForAssertion(() =>
            {
                Assert.Contains("Score : 100", component.Markup);
            }, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task Game_Should_EndGame_OnWrongAnswer()
        {
            JSInterop.Setup<string>("localStorage.getItem", "favorites");

            // Arrange: Render the whole page component that includes GameInterface and the game over display.
            var gamePage = RenderComponent<FactRush.Pages.Game>();

            // Wait for the wrong answer button ("Wrong Answer") to be enabled.
            gamePage.WaitForAssertion(() =>
            {
                var buttons = gamePage.FindAll("button");
                Assert.Contains(buttons, btn => btn.TextContent.Contains("Wrong Answer") && !btn.HasAttribute("disabled"));
            }, TimeSpan.FromSeconds(7));

            // Act: Simulate a click on the wrong answer.
            var wrongButton = gamePage.FindAll("button").First(btn => btn.TextContent.Contains("Wrong Answer"));
            wrongButton.Click();

            // Assert: The game over message should appear.
            gamePage.WaitForAssertion(() =>
            {
                Assert.Contains("Game Over!", gamePage.Markup);
            }, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task Home_Should_Display_TopScore()
        {
            //Setup<String>("localStorage.getItem", "topScores");
            JSInterop.Setup<string>("localStorage.getItem", "topScores");
            // Arrange: Render the Home page
            var gameComponent = RenderComponent<FactRush.Pages.Home>();

            // Assert: Vérifie que "Arnaud" est bien dans le rendu du composant
            gameComponent.WaitForAssertion(() =>
            {
                Assert.Contains("pts", gameComponent.Markup);
            }, TimeSpan.FromSeconds(5));
        }


        public new void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
