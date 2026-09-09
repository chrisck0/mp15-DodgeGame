using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StimPack : MonoBehaviour
{
    [SerializeField] private float _moveSpeedIncrease = 5f;
    [SerializeField] private float _cooldownDecrease = 0.05f;
    [SerializeField] private int _healthDecrease = 10;
    [SerializeField] private float _buffTime = 20f;

    public float GetMoveSpeedIncrease()
    {
        return _moveSpeedIncrease;
    }

    public float GetCooldownDecrease()
    {
        return _cooldownDecrease;
    }

    public int GetHealthDecrease()
    {
        return _healthDecrease;
    }

    public float GetBuffTime()
    {
        return _buffTime;
    }
}