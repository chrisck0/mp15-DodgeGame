/*using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerBackup : MonoBehaviour
{
    
    
    [SerializeField] private Button _mainMenuButton;
    


    private void OnEnable() => Init();
    private void OnDisable() => UnbindButtonEvents();

    private void Update()
    {
        UpdateElapsedGameTime();
        UpdateGameTimer();
    }

    private void Init()
    {
        LockCursor();
        BindButtonEvents();
        _gameTimer = _totalGameTime;
    }

    public void AddDamageable(IDamageable damageable)
    {
        if (damageable is PlayerController)
        {
            _player = damageable as PlayerController;
        }
        _damageables.Add(damageable);
    }

    public void RemoveDamageable(IDamageable damageable)
    {
        _damageables.Remove(damageable);
    }

    public void NotifyDeath(IDamageable attacker, IDamageable dead)
    {
        if (dead is PlayerController)
        {
            _winGame = false;
            GameOver();
            return;
        }

        if (_damageables.Count == 1)
        {
            _winGame = true;
            GameOver();
            return;
        }

        _killlogUIController.DisplayKillLog(attacker, dead);
    }

    private void UpdateElapsedGameTime()
    {
        if (!_isPlaying) return;

        if (_gameTimer <= 0)
        {
            _winGame = false;
            GameOver();
            return;
        }
        _gameTimer -= Time.deltaTime;
    }

    private void UpdateGameTimer()
    {
        int time = (int)_gameTimer;
        _timerText.text = $"{time}";
    }
    
    private void GameOver()
    {
        if (!_isPlaying) return;

        _isPlaying = false;

        UnlockCursor();

        if (_winGame)
        {
            _gameOverText.text = "Victory!";
        }
        else
        {
            _gameOverText.text = "Game Over";
        }

        _gameOverUI.SetActive(true);
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadScene(0);
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
*/