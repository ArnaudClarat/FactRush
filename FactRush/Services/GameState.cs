namespace FactRush.Services
{
    /// <summary>
    /// Service for storing and sharing game state between components.
    /// </summary>
    public class GameState
    {
        private bool _gameOver = false;
        /// <summary>
        /// Gets or sets the player's name.
        /// </summary>
        public string PlayerName { get; set; } = "";

        /// <summary>
        /// Gets or sets the player's score.
        /// </summary>
        public int Score { get; set; } = 0;

        /// <summary>
        /// Indicates whether the game is over.
        /// </summary>
        public bool GameOver
        {
            get => _gameOver;
            set
            {
                _gameOver = value;
                NotifyStateChanged();
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
