using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CardMatchGame
{
    public struct CardData
    {
        public int cardID;
        public Sprite cardSprite;
    }

    public class Card : MonoBehaviour
    {
        private const float COLOR_FADE_OFFSET = 0.7f;
        [SerializeField] private Image frontImage;
        [SerializeField] private Image backImage;
        [SerializeField] private Image cardImageComponent;
        [SerializeField] private AudioClip cardFlipAudio;
        [SerializeField] private float flipDuration = 0.15f;


        private bool isRevealed = false;
        
        public Sprite CardSprite { get; private set; }
        public int CardID { get; private set; }

        public bool IsMatched { get; private set; }

        public Action<Card> OnCardFlipped;

        public void Initialize(CardData data)
        {
            CardID = data.cardID;
            CardSprite = data.cardSprite;
            cardImageComponent.sprite = CardSprite;
        }

        public void FlashOnce(float delayInSeconds)
        {
           StartCoroutine(FlashForSeconds(delayInSeconds));
        }

        public void OnCardClicked()
        {
            if (isRevealed) return;

            Flip(TriggerOnCardFlippedEvent);
            SoundManager.GetInstance().PlayAudio(cardFlipAudio);
        }

        public void RevealCard()
        {
            cardImageComponent.sprite = CardSprite;
            isRevealed = true;
        }

        public void HideCard()
        {
            cardImageComponent.sprite = null;
            isRevealed = false;
        }

        public void MatchFound()
        {
            // Add logic for matched cards (e.g., disable further interaction)
            IsMatched = true;
            FadeCardColor();
            SetInteractable(false);
        }

        private void SetInteractable(bool value)
        {
            GetComponent<Button>().interactable = value;
        }

        public void Flip(Action<bool> OnComplete = null)
        {
            StartCoroutine(FlipRoutine(true, OnComplete));
        }

        public void FlipBack()
        {
            if (IsMatched) return;
            StartCoroutine(FlipRoutine(false, TriggerOnCardFlippedEvent));
        }

        IEnumerator FlipRoutine(bool faceUp, Action<bool> OnFlipComplete)
        {
            yield return Rotate(0, 90);
            frontImage.gameObject.SetActive(faceUp);
            cardImageComponent.sprite = CardSprite;
            backImage.gameObject.SetActive(!faceUp);
            yield return Rotate(90, 0);

            if (OnFlipComplete != null)
            {
                OnFlipComplete.Invoke(faceUp);
            }
        }

        private void TriggerOnCardFlippedEvent(bool faceUp)
        {
            if (faceUp)
            {

                RevealCard();
                OnCardFlipped?.Invoke(this);
            }
            else
            {
                HideCard();
            }
        }

        IEnumerator Rotate(float from, float to)
        {
            float t = 0;
            while (t < flipDuration)
            {
                t += Time.deltaTime;
                float angle = Mathf.Lerp(from, to, t / flipDuration);
                transform.localRotation = Quaternion.Euler(0, angle, 0);
                yield return null;
            }
        }

        

        private void OnDestroy()
        {
            OnCardFlipped = null;
        }

        public IEnumerator FlashForSeconds(float waitTime)
        {
            frontImage.gameObject.SetActive(true);
            backImage.gameObject.SetActive(false);
            SetInteractable(false);
            yield return new WaitForSeconds(waitTime);
            FlipBack();
            SetInteractable(true);
        }

        public void SetFaceDownInstant()
        {
            IsMatched = false;
            frontImage.gameObject.SetActive(false);
            backImage.gameObject.SetActive(true);
        }

        public void SetMatchedInstant()
        {
            frontImage.gameObject.SetActive(true);
            backImage.gameObject.SetActive(false);
            MatchFound();
        }

        private void FadeCardColor()
        {
            cardImageComponent.color = cardImageComponent.color * COLOR_FADE_OFFSET;
        }
    }
}