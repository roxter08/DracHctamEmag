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

        private GridManager gridManager;
        private MatchManager matchManager;
        private GameLogicManager gameLogicManager;
        private ScoreManager scoreController;
        private UIViewManager viewController;
        private GameCallbacks gameCallbacks;

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
            ValidateProperCardCount();
            int totalNumberOfPairs = (int)(rows * columns * 0.5f);

            gameCallbacks = new GameCallbacks();
            gridManager = new GridManager(gridRoot, rows, columns, cardImages, cardPrefab, gameCallbacks);
            matchManager = new MatchManager(matchedAudio, mismatchedAudio, gameCallbacks);
            gameLogicManager = new GameLogicManager(totalNumberOfPairs, gameOverAudio, gameCallbacks);
            scoreController = new ScoreManager(10, gameCallbacks);
            viewController = new UIViewManager(uiRootTransform);
            SoundManager.GetInstance().Initialize(audioSources);
        }

        private void OnEnable()
        {
            gameLogicManager.AddListener();
            scoreController.AddListener();
            viewController.AddListener();
            GameCallbacks.OnCardGenerated += matchManager.RegisterCard;
        }

        private void OnDisable()
        {
            gameLogicManager.RemoveListener();
            scoreController.RemoveListener();
            viewController.RemoveListener();
            GameCallbacks.OnCardGenerated -= matchManager.RegisterCard;
        }

        private void Start()
        {
            gridManager.Initialize();
            viewController.AddListenerOnExitButtonClicked(ExitGame);
            viewController.AddListenerOnRestartButtonClicked(RestartGame);
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

        private void Update()
        {
            matchManager.CheckForMatch();
        }
    }
}