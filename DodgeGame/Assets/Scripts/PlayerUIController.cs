using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _magazineText;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _grenadeText;
    [SerializeField] private TextMeshProUGUI _killLogText;
    [SerializeField] private TextMeshProUGUI _gameTimerText;
    [SerializeField] private Timer _killLogTimer;

    private GameTimer _gameTimer;

    // --------------
    private void Start() => Init();
    private void Update() => RefreshGameTimer();
    private void OnEnable() => BindTimerEvents();
    private void OnDisable() => UnbindTimerEvents();
    // --------------

    private void Init()
    {
        _killLogText.gameObject.SetActive(false);
        _gameTimer = GameStateManager.Instance.GameTimer;
    }

    private void BindTimerEvents()
    {
        _killLogTimer.OnTimerEnd += OnKillLogTimerEnd;
    }

    private void UnbindTimerEvents()
    {
        _killLogTimer.OnTimerEnd += OnKillLogTimerEnd;
    }

    public void RefreshMagazineUI(int currentMagazine, int maxMagazine)
    {
        _magazineText.text = $"{currentMagazine} / {maxMagazine}";
    }

    public void RefreshHealthUI(int health)
    {
        _healthText.text = $"HP : {health}";
    }

    public void RefreshGrenadeUI(int currentGrenadeNumber, int maxGrenadeNumber)
    {
        _grenadeText.text = $"{currentGrenadeNumber} / {maxGrenadeNumber}";
    }

    public void RefreshGameTimer()
    {
        int time = (int)_gameTimer.Time;
        _gameTimerText.text = $"{time}";
    }

    public void RefreshKillLog(IDamageable attacker, IDamageable dead)
    {
        _killLogText.text = $"{attacker.GameObject.name} has killed {dead.GameObject.name}";
        _killLogTimer.gameObject.SetActive(true);
    }

    private void OnKillLogTimerEnd()
    {
        _killLogTimer.gameObject.SetActive(false);
    }
}