using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
    public event Action<Vector2> OnMove;
    public event Action OnDiveRoll;

    private float _diveRollDelay = 2.367f;

    private Coroutine _routine;
    private WaitUntil _waitUntilDiveRollInput;
    private WaitForSeconds _waitDiveRollDelay;
    private Vector2 _prevMovement;

    private void Awake() => CacheComponents();

    private void Update()
    {
        SetMove();
    }

    private void SetMove()
    {
        if (_routine != null) return;

        if (Input.GetKeyDown(KeyCode.Space))
        { 
            _routine = StartCoroutine(DiveRollRoutine());
            return;
        }

        Vector2 movement = GetMovement();
        if (movement == _prevMovement) return;

        OnMove?.Invoke(movement);
        _prevMovement = movement;
    }

    private IEnumerator DiveRollRoutine()
    {
        OnDiveRoll?.Invoke();
        yield return _waitDiveRollDelay;
        _routine = null;
    }

    private Vector2 GetMovement()
    {
        return new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );
    }

    private void CacheComponents()
    {
        _waitDiveRollDelay = new WaitForSeconds(_diveRollDelay);
    }
}
