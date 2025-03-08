using System.Text.Json;
using Microsoft.JSInterop;

namespace FactRush.Services
{
    /// <summary>
    /// Represents a player's score entry.
    /// </summary>
    /// <param name="PlayerName">The name of the player.</param>
    /// <param name="Score">The number of points the player has achieved.</param>
    public record ScoreEntry(string PlayerName, int Score);

    /// <summary>
    /// Service for managing player scores.
    /// This service persists the top scores in the browser's local storage.
    /// Call InitializeAsync() before using the service to load any stored data.
    /// </summary>
    public class TopScoreService
    {
        // Constant key used to store and retrieve top scores in local storage.
        private const string TopScoresKey = "topScores";

        // The local storage service dependency for persisting data.
        private readonly LocalStorageService _localStorageService;

        /// <summary>
        /// Gets the list of top scores.
        /// </summary>
        public List<ScoreEntry> TopScores { get; private set; }

        /// <summary>
        /// An event to notify when the TopScores property changes.
        /// UI components can subscribe to this event to trigger re-rendering.
        /// </summary>
        public event Action? OnChange;

        /// <summary>
        /// Constructs the TopScoreService with the given LocalStorageService.
        /// The TopScores list is initially set to default values.
        /// </summary>
        /// <param name="localStorageService">An instance of LocalStorageService for data persistence.</param>
        public TopScoreService(LocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
            // Initialize with default scores.
            TopScores = new List<ScoreEntry>
            {
                new ScoreEntry("Alice", 1200),
                new ScoreEntry("Bob", 1100),
                new ScoreEntry("Charlie", 1000)
            };
        }

        /// <summary>
        /// Asynchronously initializes the TopScoreService by loading stored scores from local storage.
        /// If no stored scores are found or if the stored list is empty, the default scores remain and are persisted.
        /// </summary>
        public async Task InitializeAsync()
        {
            // Attempt to retrieve stored top scores from local storage.
            var storedScores = await _localStorageService.GetItemAsync<List<ScoreEntry>>(TopScoresKey);

            // If storedScores is not null and contains at least one entry, use it.
            // Otherwise, persist the default scores.
            if (storedScores != null && storedScores.Count > 0)
            {
                TopScores = storedScores;
            }
            else
            {
                // Persist the default scores since nothing is stored.
                await _localStorageService.SetItemAsync(TopScoresKey, TopScores);
            }

            // Notify subscribers that the state has changed.
            OnChange?.Invoke();
        }

        /// <summary>
        /// Asynchronously adds a new score entry, retains only the top 10 scores,
        /// and persists the updated list to local storage.
        /// </summary>
        /// <param name="entry">The new score entry to add.</param>
        public async Task AddScoreAsync(ScoreEntry entry)
        {
            // Add the new score entry.
            TopScores.Add(entry);

            // Order the list by descending score and keep only the top 10 scores.
            TopScores = TopScores.OrderByDescending(s => s.Score).Take(10).ToList();

            // Persist the updated top scores list to local storage.
            await _localStorageService.SetItemAsync(TopScoresKey, TopScores);

            // Notify subscribers that the state has changed.
            OnChange?.Invoke();
        }
    }
}
