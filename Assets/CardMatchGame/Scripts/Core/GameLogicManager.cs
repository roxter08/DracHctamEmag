using UnityEngine;

namespace CardMatchGame
{
    public class GameLogicManager : IEventListener
    {
        private int totalNumberOfPairs;
        public GameCallbacks gameCallbacks;
        private AudioClip gameOverAudioClip;
        private MatchManager matchManager;

        public bool IsGameComplete { get; private set; }
        public GameLogicManager(int numberOfCardPairs, AudioClip gameOverAudio, MatchManager matchManager, GameCallbacks gameCallbacks)
        {
            this.matchManager = matchManager;
            totalNumberOfPairs = numberOfCardPairs;
            this.gameCallbacks = gameCallbacks;
            this.gameOverAudioClip = gameOverAudio;
        }
        private void CheckWinLoseCondition()
        {
            if (matchManager.TotalCardsMatched == totalNumberOfPairs)
            {
                IsGameComplete = true;
                gameCallbacks.RaiseGameCompletionEvent();
                SoundManager.GetInstance().PlayAudio(gameOverAudioClip);
            }
        }

        public void AddListener()
        {
            GameCallbacks.OnMatchFound += CheckWinLoseCondition;

        }

        public void RemoveListener()
        {
            GameCallbacks.OnMatchFound -= CheckWinLoseCondition;
        }
    }
}
