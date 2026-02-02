using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardMatchGame
{
    public class MatchManager
    {
        private Card firstSelectedCard;
        private Card secondSelectedCard;
        private Queue<CardMatchHandler> cardMatchHandlerQueue;
        private GameCallbacks gameCallbacks;

        //Audio
        private AudioClip matchAudioClip;
        private AudioClip mismatchAudioClip;

        public MatchManager(AudioClip matchAudio, AudioClip misMatchAudio, GameCallbacks gameCallbacks)
        {
            this.gameCallbacks = gameCallbacks;
            cardMatchHandlerQueue = new Queue<CardMatchHandler>();
            this.matchAudioClip = matchAudio;
            this.mismatchAudioClip = misMatchAudio;
        }

        public void RegisterCard(Card card)
        {
            card.OnCardFlipped += CardSelected;
        }

        public void CardSelected(Card selectedCard)
        {
            if (firstSelectedCard == null)
            {
                firstSelectedCard = selectedCard;
            }
            else if (secondSelectedCard == null && selectedCard != firstSelectedCard)
            {
                secondSelectedCard = selectedCard;
                cardMatchHandlerQueue.Enqueue(new CardMatchHandler(firstSelectedCard, secondSelectedCard));
                ResetSelectedCards();
            }
        }

        private void ResetSelectedCards()
        {
            firstSelectedCard = null;
            secondSelectedCard = null;
        }

        public async void CheckForMatch()
        {
            for (int i = cardMatchHandlerQueue.Count - 1; i >= 0; i--)
            {
                CardMatchHandler cardPair = cardMatchHandlerQueue.Dequeue();
                await cardPair.CheckForMatch((value) =>
                {
                    if(value.HasMatched)
                    {
                        gameCallbacks.RaiseMatchFoundEvent();
                        SoundManager.GetInstance().PlayAudio(matchAudioClip);
                    }
                    else
                    {
                        gameCallbacks.RaiseMismatchFoundEvent();
                        SoundManager.GetInstance().PlayAudio(mismatchAudioClip);
                    }
                });
            }
        }
    }
}