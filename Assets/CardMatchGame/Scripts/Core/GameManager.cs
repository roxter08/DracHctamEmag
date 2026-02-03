using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CardMatchGame
{
    public class GameManager : MonoBehaviour
    {
        //Game Related Data
        [Header("GAME DATA")]
        [SerializeField] int rows;
        [SerializeField] int columns;
        [SerializeField] Card cardPrefab; 
        [SerializeField] List<Sprite> cardImages; 

        [Space(30)]

        //UI Related Data
        [Header("UI DATA")]
        [SerializeField] GridLayoutGroup gridRoot;
        [SerializeField] Transform uiRootTransform;

        [Space(30)]

        //Audio Related Data
        [Header("AUDIO DATA")]
        [SerializeField] AudioSource[] audioSources;
        [SerializeField] AudioClip matchedAudio;
        [SerializeField] AudioClip mismatchedAudio;
        [SerializeField] AudioClip gameOverAudio;

        private List<CardData> cardDataSet;
        private GridManager gridManager;
        private MatchManager matchManager;
        private GameLogicManager gameLogicManager;
        private ScoreManager scoreController;
        private UIViewManager viewController;
        private GameCallbacks gameCallbacks;
        private SaveData saveData;

        private void OnValidate()
        {
            ValidateProperCardCount();
        }

        /// <summary>
        /// This function is a precaution check
        /// If total cards are not even a pair match cannot occur
        /// Hence the game will throw errors
        /// </summary>
        private void ValidateProperCardCount()
        {
            if (rows * columns % 2f != 0)
            {
                Debug.LogError("TOTAL NUMBER OF CARDS MUST BE EVEN FOR MATCHING \n Provide correct Row and Column values accordingly!");
            }
        }

        private void Awake()
        {
            cardDataSet = CreateDataSetFromImages(cardImages);

            ValidateProperCardCount();
            int totalNumberOfPairs = (int)(rows * columns * 0.5f);

            gameCallbacks = new GameCallbacks();
            gridManager = new GridManager(gridRoot, rows, columns, cardDataSet, cardPrefab, gameCallbacks);
            matchManager = new MatchManager(matchedAudio, mismatchedAudio, gameCallbacks);
            gameLogicManager = new GameLogicManager(totalNumberOfPairs, gameOverAudio, matchManager, gameCallbacks);
            scoreController = new ScoreManager(10, gameCallbacks);
            viewController = new UIViewManager(uiRootTransform);
            SoundManager.GetInstance().Initialize(audioSources);
        }

        

        //Put it inside cards manager
        private List<CardData> CreateDataSetFromImages(List<Sprite> images)
        {
            cardDataSet = new List<CardData>(images.Count);
            for (int i = 0; i < images.Count; i++)
            {
                int index = i;
                cardDataSet.Add(new CardData()
                {
                    cardID = index,
                    cardSprite = cardImages[i]
                });
            }
            return cardDataSet;
        }

        private void OnEnable()
        {
            matchManager.AddListener();
            gameLogicManager.AddListener();
            scoreController.AddListener();
            viewController.AddListener();
            GameCallbacks.OnCardGenerated += matchManager.RegisterCard;
        }

        private void OnDisable()
        {
            matchManager.RemoveListener();
            gameLogicManager.RemoveListener();
            scoreController.RemoveListener();
            viewController.RemoveListener();
            GameCallbacks.OnCardGenerated -= matchManager.RegisterCard;
        }

        private void Start()
        {
            if (SaveLoadManager.Instance.LoadGame(out SaveData saveData))
            {
                this.saveData = saveData;
            }
            gridManager.Initialize(saveData);
            viewController.AddListenerOnExitButtonClicked(ExitGame);
            viewController.AddListenerOnRestartButtonClicked(SaveGameProgress);
            viewController.AddListenerOnRestartButtonClicked(RestartGame);
        }

        private void Update()
        {
            matchManager.CheckForMatch();
        }

        private void ExitGame()
        {
            //Exit Game
            Application.Quit();
        }
        private void RestartGame()
        {
            //Reload Level
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnApplicationQuit()
        {
           SaveGameProgress();
        }

        private void SaveGameProgress()
        {
            if (gameLogicManager.IsGameComplete)
            {
                SaveLoadManager.Instance.Clear();
            }
            else
            {
                SaveLoadManager.Instance.SaveGame(gridManager.CardsList, gridManager.Rows, gridManager.Columns, scoreController.GetScore(), scoreController.GetTurnsTaken(), matchManager.TotalCardsMatched);
            }
        }
    }
}