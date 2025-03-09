using FactRush.Models;
using FactRush.Services;
using Moq;

namespace FactRushTest
{
    /// <summary>
    /// Tests for the Question model and QuestionService.
    /// </summary>
    public class QuestionTest
    {
        private readonly QuestionService _questionService;
        private readonly Mock<ILocalStorageService> _localStorageService;

        /// <summary>
        /// Constructor that initializes the QuestionService with a new HttpClient.
        /// </summary>
        public QuestionTest()
        {
            _localStorageService = new Mock<ILocalStorageService>();
            _localStorageService
                .Setup(s => s.GetItemAsync<List<Question>>("favorites"))
                .ReturnsAsync([]);
            _questionService = new QuestionService(new HttpClient(), _localStorageService.Object);
        }

        /// <summary>
        /// Verifies that the properties of a Question object can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void Properties_Should_SetAndGet_CorrectValues()
        {
            // Arrange: Create a Question with specific values.
            var question = new Question
            {
                Category = "Science",
                Type = "multiple",
                Difficulty = "medium",
                Text = "What is the chemical symbol for water?",
                CorrectAnswer = "H2O",
                IncorrectAnswers = ["O2", "HO", "H3O"],
                IsFavorite = false
            };

            // Act & Assert: Verify each property has the expected value.
            Assert.Equal("Science", question.Category);
            Assert.Equal("multiple", question.Type);
            Assert.Equal("medium", question.Difficulty);
            Assert.Equal("What is the chemical symbol for water?", question.Text);
            Assert.Equal("H2O", question.CorrectAnswer);
            Assert.Equal(["O2", "HO", "H3O"], question.IncorrectAnswers);
            Assert.False(question.IsFavorite);
        }

        /// <summary>
        /// Tests that HTML entities in a Question are properly decoded.
        /// </summary>
        [Fact]
        public void DecodeHtmlEntities_Should_DecodeAllFields()
        {
            // Arrange: Create a Question with HTML-encoded strings.
            var question = new Question
            {
                Text = "What is &quot;HTML&quot;?",
                CorrectAnswer = "&lt;HyperText Markup Language&gt;",
                IncorrectAnswers = ["&lt;Home Tool Markup Language&gt;", "&lt;Hyperlinks Text Markup Language&gt;", "&lt;High Tension Mallet Limbo&gt;"]
            };

            // Act: Decode HTML entities.
            question.DecodeHtmlEntities();

            // Assert: Verify that the fields are correctly decoded.
            Assert.Equal("What is \"HTML\"?", question.Text);
            Assert.Equal("<HyperText Markup Language>", question.CorrectAnswer);
            Assert.Equal(["<Home Tool Markup Language>", "<Hyperlinks Text Markup Language>", "<High Tension Mallet Limbo>"], question.IncorrectAnswers);
        }

        /// <summary>
        /// Tests that the ToString method returns the correct formatted string.
        /// </summary>
        [Fact]
        public void ToString_Should_ReturnFormattedString()
        {
            // Arrange: Create a Question with sample text, correct answer, and difficulty.
            var question = new Question
            {
                Text = "What is the capital of France?",
                CorrectAnswer = "Paris",
                Difficulty = "easy"
            };

            // Act: Retrieve the formatted string.
            var result = question.ToString();

            // Assert: Check that the formatted string matches the expected format.
            Assert.Equal("Text: \"What is the capital of France?\", CorrectAnswer: \"Paris\", Difficulty: \"easy\"", result);
        }

        /// <summary>
        /// Tests that a new Question instance has default values (empty strings and an empty array).
        /// </summary>
        [Fact]
        public void DefaultValues_Should_BeEmptyStrings_AndEmptyArray()
        {
            // Arrange: Create a new Question with no initialization.
            var question = new Question();

            // Assert: All string properties should be empty, and IncorrectAnswers should be empty.
            Assert.Equal("", question.Category);
            Assert.Equal("", question.Type);
            Assert.Equal("", question.Difficulty);
            Assert.Equal("", question.Text);
            Assert.Equal("", question.CorrectAnswer);
            Assert.Empty(question.IncorrectAnswers);
        }

        /// <summary>
        /// Tests that calling DecodeHtmlEntities on an empty Question does not cause errors.
        /// </summary>
        [Fact]
        public void DecodeHtmlEntities_Should_HandleEmptyFields()
        {
            // Arrange: Create a Question with all fields empty.
            var question = new Question();

            // Act: Call DecodeHtmlEntities.
            question.DecodeHtmlEntities();

            // Assert: The fields remain empty.
            Assert.Equal("", question.Text);
            Assert.Equal("", question.CorrectAnswer);
            Assert.Empty(question.IncorrectAnswers);
        }

        /// <summary>
        /// Tests that GenerateAnswerChoices returns ["True", "False"] for a boolean-type question.
        /// </summary>
        [Fact]
        public void GenerateAnswerChoices_Should_ReturnTrueFalse_ForBooleanType()
        {
            // Arrange: Create a boolean-type Question.
            var question = new Question
            {
                Type = "boolean",
                CorrectAnswer = "True",
                IncorrectAnswers = []
            };

            // Act: Generate answer choices.
            var choices = Question.GenerateAnswerChoices(question);

            // Assert: The choices should contain exactly "True" and "False".
            Assert.Equal(2, choices.Count);
            Assert.Contains("True", choices);
            Assert.Contains("False", choices);
        }

        /// <summary>
        /// Tests that GenerateAnswerChoices returns all expected choices for a multiple-choice question.
        /// </summary>
        [Fact]
        public void GenerateAnswerChoices_Should_ContainAllChoices_ForMultipleType()
        {
            // Arrange: Create a multiple-choice Question.
            var question = new Question
            {
                Type = "multiple",
                CorrectAnswer = "Paris",
                IncorrectAnswers = ["London", "Berlin", "Madrid"]
            };

            // Act: Generate the answer choices.
            var choices = Question.GenerateAnswerChoices(question);

            // Assert: Verify that the choices include the correct answer and all incorrect answers (order is random).
            Assert.Equal(4, choices.Count);
            Assert.Contains("Paris", choices);
            Assert.Contains("London", choices);
            Assert.Contains("Berlin", choices);
            Assert.Contains("Madrid", choices);
        }

        /// <summary>
        /// Integration test: Verifies that LoadQuestions returns the expected number of valid Question instances,
        /// and that HTML entities are properly decoded.
        /// </summary>
        [Fact]
        public async Task LoadQuestions_Should_ReturnQuestions()
        {
            // Arrange: Specify the number of questions to load.
            int amount = 5;

            // Act: Load questions using the QuestionService.
            var questions = await _questionService.LoadQuestions(amount);

            // Assert: Ensure the returned array is not null, contains the correct number of questions,
            // and each question is valid (with HTML entities decoded).
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

        /// <summary>
        /// Tests that LoadQuestions throws an ArgumentOutOfRangeException when the amount is less than or equal to zero.
        /// </summary>
        [Fact]
        public async Task LoadQuestions_Should_ThrowArgumentOutOfRangeException_When_AmountIsLessThanOrEqualToZero()
        {
            // Arrange: Define an invalid amount.
            int invalidAmount = 0;

            // Act & Assert: Verify that calling LoadQuestions with an invalid amount throws the expected exception.
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _questionService.LoadQuestions(invalidAmount));
        }

        [Fact]
        public async Task SetIsFavorite_Should_SetIsFavorite_ToTrue_WhenFavoritesContainMatchingQuestion()
        {
            // Arrange
            var question = new Question { Text = "Test Question" };
            var favorites = new List<Question>{question};

            var localStorageServiceMock = new Mock<ILocalStorageService>();
            localStorageServiceMock
                .Setup(s => s.GetItemAsync<List<Question>>("favorites"))
                .ReturnsAsync(favorites);

            // Act
            await question.SetIsFavorite(localStorageServiceMock.Object);

            // Assert
            Assert.True(question.IsFavorite);
        }

        [Fact]
        public async Task SetIsFavorite_Should_SetIsFavorite_ToFalse_WhenFavoritesDoNotContainMatchingQuestion()
        {
            // Arrange
            var question = new Question { Text = "Test Question" };
            var favorites = new List<Question>{new() { Text = "Another Question" } };

            var localStorageServiceMock = new Mock<ILocalStorageService>();
            localStorageServiceMock
                .Setup(s => s.GetItemAsync<List<Question>>("favorites"))
                .ReturnsAsync(favorites);

            // Act
            await question.SetIsFavorite(localStorageServiceMock.Object);

            // Assert
            Assert.False(question.IsFavorite);
        }
    }
}
