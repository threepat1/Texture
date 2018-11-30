using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Tetris
{
    public class GameManager : MonoBehaviour
    {
        public UnityEvent onGameOver;
        public bool gameOver = false;

        public static GameManager Instance = null;
        void Awake()
        {
            if (Instance == null)
                Instance = this;    
        }

        public void GameOver()
        {
            gameOver = true;
            // If there are functions subscribed
            if (onGameOver != null)
            {
                // Invoke all subscribed functions
                onGameOver.Invoke();
            }
        }

        public void ResetGame()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }
}