using UnityEngine;

namespace CardMatchGame
{
    public class GameLogicManager : IEventListener
    {
        private int totalCardsMatched;
        private int totalNumberOfPairs;
        public GameCallbacks gameCallbacks;
        private AudioClip gameOverAudioClip;
        public GameLogicManager(int numberOfCardPairs, AudioClip gameOverAudio, GameCallbacks gameCallbacks)
        {
            totalNumberOfPairs = numberOfCardPairs;
            this.gameCallbacks = gameCallbacks;
            this.gameOverAudioClip = gameOverAudio;
        }
        private void CheckWinLoseCondition()
        {
            totalCardsMatched++;
            if (totalCardsMatched == totalNumberOfPairs)
            {
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
