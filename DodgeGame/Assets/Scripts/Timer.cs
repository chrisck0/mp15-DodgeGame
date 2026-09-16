using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float _maxTime;

    public event Action OnTimerEnd;

    private Coroutine _routine;

    private void OnEnable() => StartCoroutine(TimeRoutine());

    private IEnumerator TimeRoutine()
    {
        yield return new WaitForSeconds(_maxTime);

        Notify();
    }

    private void Notify()
    {
        OnTimerEnd?.Invoke();
    }
}
