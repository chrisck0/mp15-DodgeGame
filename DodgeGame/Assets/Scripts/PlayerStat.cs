using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField] private int MaxHealth;
    [field: SerializeField] public float MoveSpeed { get; set; }
    [field: SerializeField] public float WeaponCooldown { get; set; }
    [field: SerializeField] public float WeaponRange { get; set; }
    [field: SerializeField] public float DetectionRange { get; set; }
    [field: SerializeField] public int Damage { get; set; }

    public event Action<int> OnHealthChanged;
    [SerializeField] private int _health;
    public int Health
    {
        get => _health;
        set
        {
            _health = value;
            if (_health < 0)
            {
                _health = 0;
                return;
            }
            OnHealthChanged?.Invoke(_health);
        }
    }
}
