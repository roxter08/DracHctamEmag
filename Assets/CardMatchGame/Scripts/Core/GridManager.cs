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

        private List<CardData> cardDataList; 
        private Card cardPrefab; 

        private GameCallbacks gameCallbacks;

        public List<Card> CardsList { get; private set; }
        public int Rows => rows;
        public int Columns => columns;  

        public GridManager(GridLayoutGroup gridLayout, int rows, int columns, List<CardData> cardDataList, Card cardPrefab, GameCallbacks gameCallbacks)
        {
            this.rows = rows;
            this.columns = columns;
            this.gridRoot = gridLayout;
            this.cardDataList = cardDataList;
            this.cardPrefab = cardPrefab;
            this.gameCallbacks = gameCallbacks;
            CardsList = new List<Card>();
        }

        public void Initialize(SaveData saveData = null)
        {
            RectTransform gridRectTransform = gridRoot.transform as RectTransform;
            if(saveData != null) 
            {
                rows = saveData.rows;
                columns = saveData.columns;
            }

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

            InitializeCards(saveData);
        }

        private void InitializeCards(SaveData saveData)
        {
            int maxCount = rows * columns;

            if(saveData == null)
            {
                List<CardData> dataSet = PickRandom(cardDataList, maxCount / 2);
                dataSet.AddRange(dataSet); // Duplicate images for pairs
                dataSet = Shuffle(dataSet); // Shuffle images
                for (int i = 0; i < maxCount; i++)
                {
                    GameObject cardObject = GameObject.Instantiate(cardPrefab.gameObject, gridRoot.transform, false);
                    Card card = cardObject.GetComponent<Card>();
                    card.Initialize(dataSet[i]);
                    CardsList.Add(card);    
                    gameCallbacks.RaiseCardGeneratedEvent(card);
                }
            }
            else
            {
                for (int i = 0; i < maxCount; i++)
                {
                    GameObject cardObject = GameObject.Instantiate(cardPrefab.gameObject, gridRoot.transform, false);
                    Card card = cardObject.GetComponent<Card>();
                    CardData savedCardData = cardDataList.Find(c => c.cardID == saveData.cardInfoList[i].ID);
                    card.Initialize(savedCardData);
                    if(saveData.cardInfoList[i].IsMatched)
                    {
                        card.SetMatchedInstant();
                    }
                    else
                    {
                        card.SetFaceDownInstant();
                    }
                    CardsList.Add(card);
                    gameCallbacks.RaiseCardGeneratedEvent(card);
                }
            }
            
        }

        List<CardData> Shuffle(List<CardData> list)
        {
            for (int i = list.Count - 1; i > 0; i--)//
            {
                int randomIndex = Random.Range(0, i + 1);
                CardData temp = list[i];
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
            return list;
        }

        List<CardData> PickRandom(List<CardData> list, int maxCount)
        {
            List<CardData> result = new();
            for (int i = 0; i < maxCount; i++)//
            {
                int randomIndex = Random.Range(0, list.Count);
                result.Add(list[randomIndex]);
            }
            return result;
        }
    }
}