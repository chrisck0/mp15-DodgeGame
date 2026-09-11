using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float _totalGameTime;
    [SerializeField] private float _maxKillLogTime;
    [SerializeField] private GameObject _killLog;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private TextMeshProUGUI _killLogText;
    [SerializeField] private TextMeshProUGUI _gameOverText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Button _mainMenuButton;

    private List<IDamageable> _damageables = new List<IDamageable>();
    private float _elapsedLogTime;
    private float _gameTimer;
    private PlayerController _player;

    private bool _isKillLogDisplayEnd => _elapsedLogTime >= _maxKillLogTime;
    private bool _isShowingKillLog => _hasKillHappened && !_isKillLogDisplayEnd;
    private bool _hasKillHappened = false;
    private bool _winGame = false;
    private bool _isPlaying = true;

    private void OnEnable() => Init();
    private void OnDisable() => UnbindButtonEvents();

    private void Update()
    {
        UpdateElapsedGameTime();
        UpdateElapsedLogTime();
        UpdateGameTimer();
        UpdateKillLog();
    }

    private void Init()
    {
        LockCursor();
        BindButtonEvents();
        _gameTimer = _totalGameTime;
        _gameOverUI.SetActive(false);
    }

    private void BindButtonEvents()
    {
        _mainMenuButton.onClick.AddListener(LoadTitleScene);
    }

    private void UnbindButtonEvents()
    {
        _mainMenuButton.onClick.RemoveListener(LoadTitleScene);
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

        DisplayKillLog(attacker, dead);
    }

    private void DisplayKillLog(IDamageable attacker, IDamageable dead)
    {
        _killLogText.text = $"{attacker.GameObject.name} has killed {dead.GameObject.name}";

        _hasKillHappened = true;
        _elapsedLogTime = 0;
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

    private void UpdateElapsedLogTime()
    {
        if (!_isShowingKillLog || !_isPlaying) return;

        _elapsedLogTime += Time.deltaTime;
    }

    private void UpdateGameTimer()
    {
        int time = (int)_gameTimer;
        _timerText.text = $"{time}";
    }

    private void UpdateKillLog()
    {
        if (!_isPlaying) return;

        if (_isShowingKillLog)
        {
            _killLogText.enabled = true;
        }
        else
        {
            _killLogText.enabled = false;
        }
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
