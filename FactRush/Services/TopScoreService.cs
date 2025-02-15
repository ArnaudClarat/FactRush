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
    /// </summary>
    public class TopScoreService
    {
        /// <summary>
        /// Gets the list of top scores.
        /// The list is initialized with some default scores.
        /// </summary>
        public List<ScoreEntry> TopScores { get; private set; } =
    [
        new("Alice", 1200),
        new("Bob", 1100),
        new("Charlie", 1000)
    ];

        /// <summary>
        /// Adds a new score entry and retains only the top 10 scores.
        /// </summary>
        /// <param name="entry">The score entry to add.</param>
        public void AddScore(ScoreEntry entry)
        {
            TopScores.Add(entry);
            TopScores = TopScores
                .OrderByDescending(s => s.Score)
                .Take(10)
                .ToList();
        }
    }

}
