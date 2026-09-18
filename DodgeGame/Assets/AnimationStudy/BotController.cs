using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using System.Threading;

public class BotController : MonoBehaviour
{
    public event Action<Vector2> OnMove;
    public event Action OnDiveRoll;

    private float _diveRollDelay = 2.367f;
    private bool _isDiveRoll;
    private Vector2 _prevMovement;
    private CancellationTokenSource _cancellationTokenSource;

    private void Start()
    {
        GetCancellationTokenSource();
    }

    private void Update()
    {
        SetMove();
    }

    private void GetCancellationTokenSource()
    {
        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        _cancellationTokenSource = new CancellationTokenSource();
    }

    private void SetMove()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DiveRoll(_cancellationTokenSource.Token).Forget();
        }

        if (_isDiveRoll) return;

        Debug.Log("SetMove");
        Vector2 movement = GetMovement();
        if (movement == _prevMovement) return;

        OnMove?.Invoke(movement);
        _prevMovement = movement;
    }

    private async UniTaskVoid DiveRoll(CancellationToken token)
    {
        OnDiveRoll?.Invoke();
        await UniTask.Delay(TimeSpan.FromSeconds(_diveRollDelay), false, PlayerLoopTiming.Update, token);
    }


    private Vector2 GetMovement()
    {
        return new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );
    }
}
