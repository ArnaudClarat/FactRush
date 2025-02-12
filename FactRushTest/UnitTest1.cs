using Bunit;
using FactRush.Pages;
using FactRush.Components;

public class GameComponentTests : TestContext
{
    [Fact]
    public void GameComponent_Should_Render_Home_Screen()
    {
        // Arrange
        var component = RenderComponent<Game>();

        // Act - Vérification du rendu initial
        var h1 = component.Find("h1");
        var button = component.Find("button");

        // Assert
        Assert.Equal("QuizRush", h1.TextContent);
        Assert.Equal("Démarrer le jeu", button.TextContent);
    }

    [Fact]
    public void GameComponentShouldStartGameOnButtonClick()
    {
        // Arrange
        var component = RenderComponent<Game>();

        // Act - Cliquer sur "Démarrer le jeu"
        component.Find("button").Click();

        // Assert - Le jeu doit avoir commencé (plus de bouton "Démarrer le jeu")
        Assert.DoesNotContain(component.Markup, "Démarrer le jeu");
    }

    [Fact]
    public async Task GameComponent_Should_Display_Question_After_Starting()
    {
        // Arrange
        var component = RenderComponent<Game>();

        // Act - Lancer le jeu
        component.Find("button").Click();

        // Simuler l'affichage d'une question
        await component.InvokeAsync(() =>
        {
            component.Instance.currentQuestion = new Question
            {
                Text = "La Terre est plate ?",
                CorrectAnswer = "False"
            };
            component.Instance.TriggerStateHasChanged();
        });

        // Assert - Vérifier si la question est affichée
        Assert.Contains("La Terre est plate ?", component.Markup);
    }

    [Fact]
    public async Task GameComponent_Should_Handle_Correct_Answer()
    {
        // Arrange
        var component = RenderComponent<Game>();

        // Act - Lancer le jeu et charger une question
        component.Find("button").Click();
        await component.InvokeAsync(() =>
        {
            component.Instance.currentQuestion = new Question
            {
                Text = "Le soleil est une étoile ?",
                CorrectAnswer = "True"
            };
            component.Instance.TriggerStateHasChanged();
        });

        // Simuler un clic sur "Vrai"
        component.FindAll("button")[0].Click();

        // Assert - Vérifier si le score a été incrémenté
        Assert.Equal(1, component.Instance.score);
    }

    [Fact]
    public async Task GameComponent_Should_End_Game_On_Wrong_Answer()
    {
        // Arrange
        var component = RenderComponent<Game>();

        // Act - Lancer le jeu et charger une question
        component.Find("button").Click();
        await component.InvokeAsync(() =>
        {
            component.Instance.currentQuestion = new Question
            {
                Text = "Le feu est bleu par défaut ?",
                CorrectAnswer = "False"
            };
            component.Instance.TriggerStateHasChanged();
        });

        // Simuler un clic sur "Vrai" (mauvaise réponse)
        component.FindAll("button")[0].Click();

        // Assert - Vérifier que la partie est terminée
        Assert.True(component.Instance.gameOver);
        Assert.Contains("Game Over!", component.Markup);
    }
}
