using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class IntEvent : UnityEvent<int>
{
}

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private IntEvent _onScoreChanged;

    private int _score;

    public static ScoreManager Instance { get; private set; }

    public IntEvent OnScoreChanged => _onScoreChanged;

    private void Awake()
    {
        SetSingleton();
    }

    public void AddScore(int amount)
    {
        _score += amount;

        _onScoreChanged.Invoke(_score);
    }

    private void SetSingleton()
    {
        Instance = this;
    }
}
