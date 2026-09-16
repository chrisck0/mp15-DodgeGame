using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float _maxTime;

    public float Time { get; private set; }
    private bool _isTimerEnd => Time <= 0;

    public event Action OnTimerEnd;

    private void OnEnable()
    {
        Time = _maxTime;
    }

    private void Update()
    {
        UpdateTime();
    }

    private void UpdateTime()
    {
        if (!GameManager.Instance.IsGameRunning) return;

        Time -= UnityEngine.Time.deltaTime;

        if (_isTimerEnd)
        {
            Notify();
        }
    }

    private void Notify()
    {
        OnTimerEnd?.Invoke();
    }
}
