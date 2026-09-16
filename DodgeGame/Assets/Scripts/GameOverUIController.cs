using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameOverUIController : MonoBehaviour
{
    [SerializeField] GameOverUI _gameOverUI;

    private bool _isGameStateManagerEventsBinded;

    private void Start()
    {
        if (!_isGameStateManagerEventsBinded)
        {
            BindGameStateManagerEvents();
        }

        _gameOverUI.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (GameStateManager.Instance == null) return;

        BindGameStateManagerEvents();
    }

    private void OnDisable() => UnbindSingletonEvents();
    
    private void BindGameStateManagerEvents()
    {
        GameStateManager.Instance.OnGameOver += GameStateManager_OnGameOver;
        _isGameStateManagerEventsBinded = true;
    }

    private void UnbindSingletonEvents()
    {
        GameStateManager.Instance.OnGameOver -= GameStateManager_OnGameOver;
        _isGameStateManagerEventsBinded = false;
    }

    private void GameStateManager_OnGameOver()
    {
        _gameOverUI.GameOver();
        _gameOverUI.gameObject.SetActive(true);
    }
}
