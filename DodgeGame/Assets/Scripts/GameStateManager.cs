using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    [SerializeField] private float _totalGameTime;
    [SerializeField] private GameUIController _killlogUIController;

    public float GameTimer { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool WinGame { get; private set; }

    private List<IDamageable> _damageables = new List<IDamageable>();
    private PlayerController _player;
    

    private void Awake() => SetSingleton();
    private void Start() => Init();
    private void Update() => UpdateElapsedGameTime();

    private void SetSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Init()
    {
        GameTimer = _totalGameTime;
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
            WinGame = false;
            GameOver();
            return;
        }

        if (_damageables.Count == 1)
        {
            WinGame = true;
            GameOver();
            return;
        }

        _killlogUIController.DisplayKillLog(attacker, dead);
    }

    private void UpdateElapsedGameTime()
    {
        if (!GameManager.Instance.IsGameRunning) return;

        if (GameTimer <= 0)
        {
            WinGame = false;
            GameOver();
            return;
        }
        GameTimer -= Time.deltaTime;
    }

    private void GameOver()
    {
        IsGameOver = true;
        GameManager.Instance.Pause();
    }
}
