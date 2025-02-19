using FactRush.Models;

public class QuestionTest
{

    [Fact]
    public void Properties_Should_SetAndGet_CorrectValues()
    {
        var question = new Question
        {
            Category = "Science",
            Type = "multiple",
            Difficulty = "medium",
            Text = "What is the chemical symbol for water?",
            CorrectAnswer = "H2O",
            IncorrectAnswers = ["O2", "HO", "H3O"]
        };

        Assert.Equal("Science", question.Category);
        Assert.Equal("multiple", question.Type);
        Assert.Equal("medium", question.Difficulty);
        Assert.Equal("What is the chemical symbol for water?", question.Text);
        Assert.Equal("H2O", question.CorrectAnswer);
        Assert.Equal(["O2", "HO", "H3O"], question.IncorrectAnswers);
    }

    [Fact]
    public void DecodeHtmlEntities_Should_DecodeAllFields()
    {
        var question = new Question
        {
            Text = "What is &quot;HTML&quot;?",
            CorrectAnswer = "&lt;HyperText Markup Language&gt;",
            IncorrectAnswers = ["&lt;Home Tool Markup Language&gt;", "&lt;Hyperlinks Text Markup Language&gt;"]
        };

        question.DecodeHtmlEntities();

        Assert.Equal("What is \"HTML\"?", question.Text);
        Assert.Equal("<HyperText Markup Language>", question.CorrectAnswer);
        Assert.Equal(["<Home Tool Markup Language>", "<Hyperlinks Text Markup Language>", "<High Tension Mallet Limbo>"], question.IncorrectAnswers);
    }

    [Fact]
    public void ToString_Should_ReturnFormattedString()
    {
        var question = new Question
        {
            Text = "What is the capital of France?",
            CorrectAnswer = "Paris",
            Difficulty = "easy"
        };

        var result = question.ToString();

        Assert.Equal("Text: \"What is the capital of France?\", CorrectAnswer: \"Paris\", Difficulty: \"easy\"", result);
    }

    [Fact]
    public void DefaultValues_Should_BeEmptyStrings_AndEmptyArray()
    {
        var question = new Question();

        Assert.Equal("", question.Category);
        Assert.Equal("", question.Type);
        Assert.Equal("", question.Difficulty);
        Assert.Equal("", question.Text);
        Assert.Equal("", question.CorrectAnswer);
        Assert.Empty(question.IncorrectAnswers);
    }

    [Fact]
    public void DecodeHtmlEntities_Should_HandleEmptyFields()
    {
        var question = new Question();

        question.DecodeHtmlEntities();

        Assert.Equal("", question.Text);
        Assert.Equal("", question.CorrectAnswer);
        Assert.Empty(question.IncorrectAnswers);
    }
}