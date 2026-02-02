namespace CardMatchGame
{
    public class ScoreManager : IEventListener
    {
        private int score;
        private int turns;
        private int scoreIncrementValue;
        private GameCallbacks gameCallbacks;

        public ScoreManager(int scoreIncrementValue, GameCallbacks gameCallbacks)
        {
            this.scoreIncrementValue = scoreIncrementValue;
            this.gameCallbacks = gameCallbacks;
        }

        public void AddListener()
        {
            GameCallbacks.OnMatchFound += UpdateScore;
            GameCallbacks.OnMatchFound += UpdateTurns;
            GameCallbacks.OnMismatchFound += UpdateTurns;
        }
        public void RemoveListener()
        {
            GameCallbacks.OnMatchFound -= UpdateScore;
            GameCallbacks.OnMatchFound -= UpdateTurns;
            GameCallbacks.OnMismatchFound -= UpdateTurns;
        }

        public int GetScore()
        {
            return score;
        }
        public int GetTurnsTaken()
        {
            return turns;
        }

        private void UpdateScore()
        {
            score += scoreIncrementValue;
            gameCallbacks.RaiseScoreUpdatedEvent(score);
        }

        /// <summary>
        /// Turn will always be updated by one
        /// </summary>
        /// <returns></returns>
        public void UpdateTurns()
        {
            turns++;
            gameCallbacks.RaiseTurnUpdateEvent(turns);
        }
    }
}