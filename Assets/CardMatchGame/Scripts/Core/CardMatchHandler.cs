using System.Threading.Tasks;
using System;

namespace CardMatchGame
{
    public class CardMatchHandler
    {
        private Card firstCard;
        private Card secondCard;
        public Card FirstCard => firstCard;
        public Card SecondCard => secondCard;

        public bool HasMatched { get; private set; }

        public CardMatchHandler(Card firstCard, Card secondCard)
        {
            this.firstCard = firstCard;
            this.secondCard = secondCard;
            HasMatched = false;
        }

        public async Task CheckForMatch(Action<CardMatchHandler> OnComplete)
        {
            await Task.Delay(100);
            if (FirstCard.CardID == SecondCard.CardID)
            {
                FirstCard.MatchFound();
                SecondCard.MatchFound();
                HasMatched = true;
            }
            else
            {
                FirstCard.FlipBack();
                SecondCard.FlipBack();
                HasMatched = false;
            }

            if (OnComplete != null)
            {
                OnComplete.Invoke(this);
            }
        }
    }
}