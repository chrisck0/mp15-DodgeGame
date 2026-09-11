using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUIController : MonoBehaviour
{
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private TextMeshProUGUI _gameOverText;


    private void Start() => Init();
    private void Update() => GameOver();
    private void OnEnable() => BindButtonEvents();
    private void OnDisable() => UnbindButtonEvents();

    private void Init()
    {
        _gameOverUI.SetActive(false);
    }

    private void BindButtonEvents()
    {
        _mainMenuButton.onClick.AddListener(LoadGameScene);
    }

    private void UnbindButtonEvents()
    {
        _mainMenuButton.onClick.RemoveListener(LoadGameScene);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(0);
    }

    private void GameOver()
    {
        if (!GameStateManager.Instance.IsGameOver) return;

        if (GameStateManager.Instance.WinGame)
        {
            _gameOverText.text = "Victory!";
        }
        else
        {
            _gameOverText.text = "Game Over";
        }

        _gameOverUI.SetActive(true);
    }
}
