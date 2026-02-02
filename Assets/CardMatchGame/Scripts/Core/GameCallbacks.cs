using System;

namespace CardMatchGame
{
    public class GameCallbacks
    {
        public static event Action OnMatchFound = delegate { };
        public static event Action OnMismatchFound = delegate { };
        public static event Action<int> OnScoreUpdated = delegate { };
        public static event Action<int> OnTurnsUpdated = delegate { };
        public static event Action<Card> OnCardGenerated = delegate { };
        public static event Action OnGameCompleted = delegate { };

        public void RaiseMatchFoundEvent()
        {
            OnMatchFound.Invoke();
        }
        public void RaiseMismatchFoundEvent()
        {
            OnMismatchFound.Invoke();
        }

        public void RaiseScoreUpdatedEvent(int newScore)
        {
            OnScoreUpdated.Invoke(newScore);
        }

        public void RaiseTurnUpdateEvent(int totalTurnsTaken)
        {
            OnTurnsUpdated.Invoke(totalTurnsTaken);
        }

        public void RaiseCardGeneratedEvent(Card card)
        {
            OnCardGenerated.Invoke(card);
        }

        public void RaiseGameCompletionEvent()
        {
            OnGameCompleted.Invoke();
        }
    }
}