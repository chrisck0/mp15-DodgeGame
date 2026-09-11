using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private float _maxKillLogTime;
    [SerializeField] private TextMeshProUGUI _killLogText;
    [SerializeField] private TextMeshProUGUI _timerText;

    private float _elapsedLogTime;
    private bool _isKillLogDisplayEnd => _elapsedLogTime >= _maxKillLogTime;
    private bool _isShowingKillLog => _hasKillHappened && !_isKillLogDisplayEnd;
    private bool _hasKillHappened = false;

    private void Update()
    {
        UpdateElapsedLogTime();
        UpdateKillLog();
        UpdateGameTimer();
    }

    public void DisplayKillLog(IDamageable attacker, IDamageable dead)
    {
        _killLogText.text = $"{attacker.GameObject.name} has killed {dead.GameObject.name}";

        _hasKillHappened = true;
        _elapsedLogTime = 0;
    }

    private void UpdateElapsedLogTime()
    {
        if (!_isShowingKillLog || !GameManager.Instance.IsGameRunning) return;

        _elapsedLogTime += Time.deltaTime;
    }

    private void UpdateKillLog()
    {
        if (!GameManager.Instance.IsGameRunning) return;

        if (_isShowingKillLog)
        {
            _killLogText.enabled = true;
        }
        else
        {
            _killLogText.enabled = false;
        }
    }

    private void UpdateGameTimer()
    {
        int time = (int)GameStateManager.Instance.GameTimer;
        _timerText.text = $"{time}";
    }
}
