using FactRush.Models;
using FactRush.Services;

namespace FactRushTest
{
    public class QuestionTest
    {
        private readonly QuestionService _questionService;

        public QuestionTest()
        {
            _questionService = new QuestionService(new HttpClient());
        }

        [Fact]
        public void Properties_Should_SetAndGet_CorrectValues()
        {
            // Arrange: create a Question with specific values.
            var question = new Question
            {
                Category = "Science",
                Type = "multiple",
                Difficulty = "medium",
                Text = "What is the chemical symbol for water?",
                CorrectAnswer = "H2O",
                IncorrectAnswers = ["O2", "HO", "H3O"]
            };

            // Act & Assert: verify each property has the expected value.
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
            // Arrange: create a Question with HTML-encoded strings.
            var question = new Question
            {
                Text = "What is &quot;HTML&quot;?",
                CorrectAnswer = "&lt;HyperText Markup Language&gt;",
                IncorrectAnswers = ["&lt;Home Tool Markup Language&gt;", "&lt;Hyperlinks Text Markup Language&gt;", "&lt;High Tension Mallet Limbo&gt;"]
            };

            // Act: decode HTML entities.
            question.DecodeHtmlEntities();

            // Assert: verify the fields are correctly decoded.
            Assert.Equal("What is \"HTML\"?", question.Text);
            Assert.Equal("<HyperText Markup Language>", question.CorrectAnswer);
            Assert.Equal(["<Home Tool Markup Language>", "<Hyperlinks Text Markup Language>", "<High Tension Mallet Limbo>"], question.IncorrectAnswers);
        }

        [Fact]
        public void ToString_Should_ReturnFormattedString()
        {
            // Arrange: create a Question with sample text, correct answer, and difficulty.
            var question = new Question
            {
                Text = "What is the capital of France?",
                CorrectAnswer = "Paris",
                Difficulty = "easy"
            };

            // Act: get the string representation.
            var result = question.ToString();

            // Assert: verify the format is as expected.
            Assert.Equal("Text: \"What is the capital of France?\", CorrectAnswer: \"Paris\", Difficulty: \"easy\"", result);
        }

        [Fact]
        public void DefaultValues_Should_BeEmptyStrings_AndEmptyArray()
        {
            // Arrange: create a new Question with no initialization.
            var question = new Question();

            // Assert: all string properties should be empty and IncorrectAnswers should be empty.
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
            // Arrange: create a Question with all empty fields.
            var question = new Question();

            // Act: call DecodeHtmlEntities on the empty question.
            question.DecodeHtmlEntities();

            // Assert: the fields remain empty and no errors occur.
            Assert.Equal("", question.Text);
            Assert.Equal("", question.CorrectAnswer);
            Assert.Empty(question.IncorrectAnswers);
        }

        [Fact]
        public void GenerateAnswerChoices_Should_ReturnTrueFalse_ForBooleanType()
        {
            // Arrange: create a boolean-type Question.
            var question = new Question
            {
                Type = "boolean",
                CorrectAnswer = "True",
                IncorrectAnswers = []
            };

            // Act: generate the answer choices.
            var choices = Question.GenerateAnswerChoices(question);

            // Assert: verify that the choices list contains exactly two entries: "True" and "False".
            Assert.Equal(2, choices.Count);
            Assert.Contains("True", choices);
            Assert.Contains("False", choices);
        }

        [Fact]
        public void GenerateAnswerChoices_Should_ContainAllChoices_ForMultipleType()
        {
            // Arrange: create a multiple-choice Question.
            var question = new Question
            {
                Type = "multiple",
                CorrectAnswer = "Paris",
                IncorrectAnswers = ["London", "Berlin", "Madrid"]
            };

            // Act: generate the answer choices.
            var choices = Question.GenerateAnswerChoices(question);

            // Assert: verify that the generated choices contain all expected answers.
            Assert.Equal(4, choices.Count);
            Assert.Contains("Paris", choices);
            Assert.Contains("London", choices);
            Assert.Contains("Berlin", choices);
            Assert.Contains("Madrid", choices);
        }

        [Fact]
        public async Task LoadQuestions_Should_ReturnQuestions()
        {
            // Arrange: specify the number of questions to load.
            int amount = 5;

            // Act: load questions from the API.
            var questions = await _questionService.LoadQuestions(amount);

            // Assert: ensure the returned array is not null, has the correct number of elements, and that each element is a valid Question.
            Assert.NotNull(questions);
            Assert.NotEmpty(questions);
            Assert.Equal(amount, questions.Length);

            foreach (var q in questions)
            {
                // Verify that each element is an instance of Question.
                Assert.IsType<Question>(q);
                // Verify that HTML entities have been decoded.
                Assert.DoesNotContain("&quot;", q.Text);
                Assert.DoesNotContain("&lt;", q.CorrectAnswer);
            }
        }

        [Fact]
        public async Task LoadQuestions_Should_ThrowArgumentOutOfRangeException_When_AmountIsLessThanOrEqualToZero()
        {
            // Arrange: define an invalid amount.
            int invalidAmount = 0;

            // Act & Assert: verify that calling LoadQuestions with an invalid amount throws the expected exception.
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _questionService.LoadQuestions(invalidAmount));
        }
    }
}
