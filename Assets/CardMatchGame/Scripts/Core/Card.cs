using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CardMatchGame
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private Image frontImage;
        [SerializeField] private Image backImage;
        [SerializeField] private Image cardImageComponent;
        [SerializeField] private AudioClip cardFlipAudio;
        [SerializeField] private float flipDuration = 0.25f;
        [SerializeField] private float flashDuration = 2f;


        [HideInInspector]
        public Sprite cardImage;
        public int cardID;

        private bool isRevealed = false;
        public bool IsFaceUp { get; private set; }
        public bool IsMatched { get; private set; }

        public Action<Card> OnCardFlipped;

        public void Initialize(CardData data)
        {
            this.cardID = data.cardID;
            cardImage = data.cardSprite;
            cardImageComponent.sprite = cardImage;

            //StartCoroutine(FlashForSeconds(flashDuration));
        }

        public void OnCardClicked()
        {
            if (isRevealed) return;

            Flip(TriggerOnCardFlippedEvent);
            SoundManager.GetInstance().PlayAudio(cardFlipAudio);
        }

        public void RevealCard()
        {
            cardImageComponent.sprite = cardImage;
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
            IsFaceUp = faceUp;
            //AudioManager.Instance.PlaySound(SoundType.Flip);

            yield return Rotate(0, 90);
            frontImage.gameObject.SetActive(faceUp);
            cardImageComponent.sprite = cardImage;
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
            Flip();
            yield return new WaitForSeconds(waitTime);
            FlipBack();
        }

        public void SetFaceDownInstant()
        {
            IsFaceUp = false;
            IsMatched = false;
            frontImage.gameObject.SetActive(false);
            backImage.gameObject.SetActive(true);
        }

        public void SetMatchedInstant()
        {
            IsFaceUp = true;
            frontImage.gameObject.SetActive(true);
            backImage.gameObject.SetActive(false);
            MatchFound();
        }
    }
}