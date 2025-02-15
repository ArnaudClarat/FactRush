using Bunit;
using FactRush.Pages;
using FactRush.Components;

public class GameComponentTest : TestContext
{
    [Fact]
    public void GameComponent_Should_Render_Home_Screen()
    {
        var component = RenderComponent<Game>();

        var h1 = component.Find("h1");
        var button = component.Find("button");

        Assert.Equal("QuizRush", h1.TextContent);
        Assert.Equal("Démarrer le jeu", button.TextContent);
    }

    [Fact]
    public void GameComponentShouldStartGameOnButtonClick()
    {
        var component = RenderComponent<Game>();

        component.Find("button").Click();

        Assert.DoesNotContain(component.Markup, "Démarrer le jeu");
    }

    [Fact]
    public async Task GameComponent_Should_Display_Question_After_Starting()
    {
        // Arrange
        var component = RenderComponent<Game>();

        component.Find("button").Click();

        await component.InvokeAsync(() =>
        {
            component.Instance.currentQuestion = new Question
            {
                Text = "La Terre est plate ?",
                CorrectAnswer = "False"
            };
        });

        Assert.Contains("La Terre est plate ?", component.Markup);
    }

    [Fact]
    public async Task GameComponent_Should_Handle_Correct_Answer()
    {
        var component = RenderComponent<Game>();

        component.Find("button").Click();
        await component.InvokeAsync(() =>
        {
            component.Instance.currentQuestion = new Question
            {
                Text = "Le soleil est une étoile ?",
                CorrectAnswer = "True"
            };
        });

        component.FindAll("button")[0].Click();

        Assert.Equal(1, component.Instance.score);
    }

    [Fact]
    public async Task GameComponent_Should_End_Game_On_Wrong_Answer()
    {
        var component = RenderComponent<Game>();

        component.Find("button").Click();
        await component.InvokeAsync(() =>
        {
            component.Instance.currentQuestion = new Question
            {
                Text = "Le feu est bleu par défaut ?",
                CorrectAnswer = "False"
            };
        });

        component.FindAll("button")[0].Click();

        Assert.True(component.Instance.gameOver);
        Assert.Contains("Game Over!", component.Markup);
    }
}
