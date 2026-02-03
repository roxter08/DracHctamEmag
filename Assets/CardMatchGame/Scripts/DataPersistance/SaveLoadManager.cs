using UnityEngine;
using System.Collections.Generic;
using System;

namespace CardMatchGame
{
    public class SaveLoadManager : MonoBehaviour
    {
        public static SaveLoadManager Instance;
        const string SAVE_KEY = "CARD_MATCH_SAVE";

        public Action OnSaveCompleted = delegate { };
        public Action<SaveData> OnLoadCompleted = delegate { };

        void Awake()
        {
            Instance = this;
        }

        public void SaveGame(List<Card> cards, int rows, int columns, int totalScore, int totalTurnsTaken, int totalCardMatches)
        {
            SaveData data = new()
            {
                rows = rows,
                columns = columns,
                cardInfoList = new()
            };

            foreach (var card in cards)
            {
                CardInfo cardInfo = new CardInfo();
                cardInfo.ID = card.cardID;
                cardInfo.IsMatched = card.IsMatched;
                data.cardInfoList.Add(cardInfo);
            }
            data.totalMatches = totalCardMatches;
            data.score = totalScore;
            data.turnsTaken = totalTurnsTaken;

            PlayerPrefs.SetString(SAVE_KEY, JsonUtility.ToJson(data));
            Debug.Log("Game Saved");
            OnSaveCompleted.Invoke();
        }

        public bool LoadGame(out SaveData saveData)
        {
            saveData = null;
            if (!PlayerPrefs.HasKey(SAVE_KEY))
            {
                Debug.Log("Save Key Not Found");
            }
            else
            {
                saveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(SAVE_KEY));
                OnLoadCompleted.Invoke(saveData);
                Debug.Log("Game Loaded");
            }
            return saveData == null;
        }

        public void Clear()
        {
            Debug.Log("Game Data Cleared");
            PlayerPrefs.DeleteKey(SAVE_KEY);
        }
    }
}