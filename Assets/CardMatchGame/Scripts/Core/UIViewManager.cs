using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMatchGame
{
    public class UIViewManager : IEventListener
    {
        private Transform uiRootTransform;
        private GameObject gameCompletionTitle;
        private TextMeshProUGUI scoreText;
        private TextMeshProUGUI turnText;

        private Button exitGameButton;
        private Button restartGameButton;

        public UIViewManager(Transform uiRoot)
        {
            uiRootTransform = uiRoot;
            gameCompletionTitle = uiRootTransform.Find("GameComplete").gameObject;
            scoreText = uiRootTransform.Find("GameHUD/ScoreBoard/ScoreInfo/Score").GetComponent<TextMeshProUGUI>();
            turnText = uiRootTransform.Find("GameHUD/ScoreBoard/TurnsInfo/Turns").GetComponent<TextMeshProUGUI>();
            exitGameButton = uiRootTransform.Find("GameHUD/MenuButtons/ExitButton").GetComponent<Button>();
            restartGameButton = uiRootTransform.Find("GameHUD/MenuButtons/RestartButton").GetComponent<Button>();

            gameCompletionTitle.SetActive(false);
        }

        public void AddListenerOnExitButtonClicked(Action action)
        {
            exitGameButton.onClick.AddListener(() => action.Invoke());
        }
        public void AddListenerOnRestartButtonClicked(Action action)
        {
            restartGameButton.onClick.AddListener(() => action.Invoke());
        }

        public void DisplayTurns(int turns)
        {
            turnText.text = turns.ToString();
        }

        public void DisplayNewScore(int value)
        {
            scoreText.text = value.ToString();
        }
        public void DisplayGameCompletion()
        {
            gameCompletionTitle.SetActive(true);
        }

        public void AddListener()
        {
            GameCallbacks.OnScoreUpdated += DisplayNewScore;
            GameCallbacks.OnGameCompleted += DisplayGameCompletion;
            GameCallbacks.OnTurnsUpdated += DisplayTurns;
        }

        public void RemoveListener()
        {
            GameCallbacks.OnScoreUpdated -= DisplayNewScore;
            GameCallbacks.OnGameCompleted -= DisplayGameCompletion;
            GameCallbacks.OnTurnsUpdated -= DisplayTurns;
        }
    }
}