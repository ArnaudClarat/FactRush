using FactRush.Models;

namespace FactRushTest
{
    public class QuestionResponseTests
    {
        [Fact]
        public void Properties_Should_SetAndGet_CorrectValues()
        {
            // Arrange: Create an array of Question objects with sample values.
            var questions = new[]
            {
                new Question { Text = "Question 1", CorrectAnswer = "Answer 1" },
                new Question { Text = "Question 2", CorrectAnswer = "Answer 2" }
            };

            // Arrange: Initialize a QuestionResponse object with the questions and a response code.
            var questionResponse = new QuestionResponse
            {
                ResponseCode = 0,
                Questions = questions
            };

            // Act & Assert: Verify that the ResponseCode and Questions properties return the expected values.
            Assert.Equal(0, questionResponse.ResponseCode);
            Assert.Equal(questions, questionResponse.Questions);
        }

        [Fact]
        public void DefaultValues_Should_BeCorrect()
        {
            // Arrange: Create a new QuestionResponse instance without initialization.
            var questionResponse = new QuestionResponse();

            // Assert: The default response code should be 0 and the Questions array should be empty.
            Assert.Equal(0, questionResponse.ResponseCode);
            Assert.Empty(questionResponse.Questions);
        }

        [Fact]
        public void ToString_Should_ReturnFormattedString_WithQuestions()
        {
            // Arrange: Create an array of Question objects with sample data.
            var questions = new[]
            {
                new Question { Text = "What is the capital of France?", CorrectAnswer = "Paris", Difficulty = "easy" },
                new Question { Text = "What is 2 + 2?", CorrectAnswer = "4", Difficulty = "easy" }
            };

            // Arrange: Initialize a QuestionResponse object with the questions.
            var questionResponse = new QuestionResponse
            {
                ResponseCode = 0,
                Questions = questions
            };

            // Act: Retrieve the formatted string from the ToString method.
            var result = questionResponse.ToString();

            // Arrange: Define the expected formatted string.
            var expected = @"ResponseCode: 0
Questions:
- Text: ""What is the capital of France?"", CorrectAnswer: ""Paris"", Difficulty: ""easy""
- Text: ""What is 2 + 2?"", CorrectAnswer: ""4"", Difficulty: ""easy""
";

            // Assert: Verify that the returned string matches the expected output.
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToString_Should_ReturnFormattedString_WhenNoQuestions()
        {
            // Arrange: Initialize a QuestionResponse object with a non-zero response code and an empty questions array.
            var questionResponse = new QuestionResponse
            {
                ResponseCode = 1,
                Questions = []
            };

            // Act: Get the string representation.
            var result = questionResponse.ToString();

            // Arrange: Define the expected string when there are no questions.
            var expected = @"ResponseCode: 1
No questions available.
";

            // Assert: Verify that the string representation is as expected.
            Assert.Equal(expected, result);
        }
    }
}
