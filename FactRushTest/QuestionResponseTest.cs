using FactRush.Components;

namespace FactRush.Tests
{
    public class QuestionResponseTests
    {
        [Fact]
        public void Properties_Should_SetAndGet_CorrectValues()
        {
            var questions = new[]
            {
                new Question { Text = "Question 1", CorrectAnswer = "Answer 1" },
                new Question { Text = "Question 2", CorrectAnswer = "Answer 2" }
            };

            var questionResponse = new QuestionResponse
            {
                ResponseCode = 0,
                Questions = questions
            };

            Assert.Equal(0, questionResponse.ResponseCode);
            Assert.Equal(questions, questionResponse.Questions);
        }

        [Fact]
        public void DefaultValues_Should_BeCorrect()
        {
            var questionResponse = new QuestionResponse();

            Assert.Equal(0, questionResponse.ResponseCode);
            Assert.Empty(questionResponse.Questions);
        }

        [Fact]
        public void ToString_Should_ReturnFormattedString_WithQuestions()
        {
            var questions = new[]
            {
                new Question { Text = "What is the capital of France?", CorrectAnswer = "Paris", Difficulty = "easy" },
                new Question { Text = "What is 2 + 2?", CorrectAnswer = "4", Difficulty = "easy" }
            };

            var questionResponse = new QuestionResponse
            {
                ResponseCode = 0,
                Questions = questions
            };

            var result = questionResponse.ToString();

            var expected = @"ResponseCode: 0
Questions:
- Text: ""What is the capital of France?"", CorrectAnswer: ""Paris"", Difficulty: ""easy""
- Text: ""What is 2 + 2?"", CorrectAnswer: ""4"", Difficulty: ""easy""
";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToString_Should_ReturnFormattedString_WhenNoQuestions()
        {
            var questionResponse = new QuestionResponse
            {
                ResponseCode = 1,
                Questions = []
            };

            var result = questionResponse.ToString();

            var expected = @"ResponseCode: 1
No questions available.
";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToString_Should_HandleNullQuestionsGracefully()
        {
            var questionResponse = new QuestionResponse
            {
                ResponseCode = 1,
                Questions = null // Explicitly set to null
            };

            // Act
            var result = questionResponse.ToString();

            // Assert
            var expected = @"ResponseCode: 1
No questions available.
";
            Assert.Equal(expected, result);
        }
    }
}
