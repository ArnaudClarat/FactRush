# FactRush

FactRush is a trivia game built using Blazor WebAssembly. This project was created as part of an Object-Oriented Programming exam.

## Table of Contents
- [Description](#description)
- [Technologies](#technologies)
- [Installation](#installation)
- [Usage](#usage)
- [Testing](#testing)
- [Acknowledgments](#acknowledgments)

## Description
FactRush is a trivia game where players answer multiple-choice questions. The game tracks the player's score and ends when a wrong answer is given. The project includes a Blazor WebAssembly frontend and a set of unit tests to ensure the functionality of the game components.

### API
FactRush uses an external trivia API to fetch questions. The API provides questions in various categories and difficulties. Here are some details about the API:

- **Categories**: The API offers questions in multiple categories such as General Knowledge, Science, History, etc.
- **Difficulties**: Questions are available in three difficulty levels: easy, medium, and hard.
- **Question Types**: The API provides multiple-choice questions.

The `QuestionService` class is responsible for interacting with the API and fetching the questions. The `FakeHttpMessageHandler` is used in the test project to mock API responses for testing purposes.

### Local Storage
- **Score**: The player's score is stored locally using the browser's local storage. This allows the game to persist the score even if the page is refreshed.
- **Favorites**: Players can add questions to their list of favorites. The favorites are also stored locally using the browser's local storage.

## Technologies
- Blazor WebAssembly
- .NET 8
- Bunit (for unit testing)
- C# 12.0
- External Trivia API

## Technologies
- Blazor WebAssembly
- .NET 8
- Bunit (for unit testing)
- C# 12.0

## Installation
1. Clone the repository: `git clone https://github.com/ArnaudClarat/FactRush.git`
3. Navigate to the project directory: `cd FactRush`
5. Restore the dependencies: `dotnet restore`

## Usage
1. Build the project: `dotnet build`
3. Run the project: `dotnet run`
5. Open your browser and navigate to `https://localhost:5001` to play the game.

## Testing
1. Navigate to the test project directory: `cd FactRushTest`
3. Run the tests: `dotnet test`

## Acknowledgments
This project was created as part of an Object-Oriented Programming exam. Special thanks to the course instructors for their guidance and support.
