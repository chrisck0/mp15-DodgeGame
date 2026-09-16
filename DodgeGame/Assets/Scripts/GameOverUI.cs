using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TextMeshProUGUI _gameOverText;

    private void OnEnable() => BindButtonEvents();
    private void OnDisable() => UnbindButtonEvents();

    private void BindButtonEvents()
    {
        _mainMenuButton.onClick.AddListener(LoadGameScene);
    }

    private void UnbindButtonEvents()
    {
        _mainMenuButton.onClick.RemoveListener(LoadGameScene);
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        if (GameStateManager.Instance.WinGame)
        {
            _gameOverText.text = "Victory!";
        }
        else
        {
            _gameOverText.text = "Game Over";
        }
    }
}
