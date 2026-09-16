using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public event Action<IDamageable, IDamageable> OnDamageableChanged;
    public event Action OnGameOver;

    public GameTimer GameTimer { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool WinGame { get; private set; }

    private List<IDamageable> _damageables = new List<IDamageable>();
    private PlayerController _player;

    // --------------
    private void Awake()
    {
        SetSingleton();
        CacheComponents();
    }

    private void Update() => UpdateElapsedGameTime();
    // --------------

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

    private void CacheComponents()
    {
        GameTimer = GetComponent<GameTimer>();
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

        OnDamageableChanged?.Invoke(attacker, dead);
    }

    private void UpdateElapsedGameTime()
    {
        if (!GameManager.Instance.IsGameRunning) return;

        if (GameTimer.Time <= 0)
        {
            WinGame = false;
            GameOver();
            return;
        }
    }

    private void GameOver()
    {
        OnGameOver?.Invoke();
        GameManager.Instance.Pause();
    }
}
