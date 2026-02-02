using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardMatchGame
{
    public class GridManager
    {
        private GridLayoutGroup gridRoot;
        private int rows;
        private int columns;

        private List<Sprite> cardImages; 
        private Card cardPrefab; 

        private GameCallbacks gameCallbacks;

        public GridManager(GridLayoutGroup gridLayout, int rows, int columns, List<Sprite> cardImages, Card cardPrefab, GameCallbacks gameCallbacks)
        {
            this.rows = rows;
            this.columns = columns;
            this.gridRoot = gridLayout;
            this.cardImages = cardImages;
            this.cardPrefab = cardPrefab;
            this.gameCallbacks = gameCallbacks;
        }

        public void Initialize()
        {
            RectTransform gridRectTransform = gridRoot.transform as RectTransform;
            if (rows > columns)
            {
                gridRoot.cellSize = new Vector3(gridRectTransform.rect.width / columns, gridRectTransform.rect.height / rows, 0);
                gridRoot.constraintCount = columns;
            }
            else
            {
                gridRoot.cellSize = new Vector3(gridRectTransform.rect.width / rows, gridRectTransform.rect.height / columns, 0);
                gridRoot.constraintCount = rows;
            }

            InitializeCards();
        }

        private void InitializeCards()
        {
            int maxCount = rows * columns;
            List<Sprite> images = PickRandom(cardImages, maxCount / 2);
            images.AddRange(images); // Duplicate images for pairs
            images = Shuffle(images); // Shuffle images
            List<Card> cards = new();
            for (int i = 0; i < maxCount; i++)
            {
                GameObject cardObject = GameObject.Instantiate(cardPrefab.gameObject, gridRoot.transform, false);
                Card card = cardObject.GetComponent<Card>();
                card.Initialize(images[i]);
                gameCallbacks.RaiseCardGeneratedEvent(card);
            }
        }

        List<Sprite> Shuffle(List<Sprite> list)
        {
            for (int i = list.Count - 1; i > 0; i--)//
            {
                int randomIndex = Random.Range(0, i + 1);
                Sprite temp = list[i];
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
            return list;
        }

        List<Sprite> PickRandom(List<Sprite> list, int maxCount)
        {
            List<Sprite> result = new();
            for (int i = 0; i < maxCount; i++)//
            {
                int randomIndex = Random.Range(0, list.Count);
                result.Add(list[randomIndex]);
            }
            return result;
        }
    }
}