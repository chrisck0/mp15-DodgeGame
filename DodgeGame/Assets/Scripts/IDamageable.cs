using System;
using UnityEngine;

public interface IDamageable
{
    public GameObject GameObject { get; }
    public int Health { get; }
    public int MaxHealth { get; }
    public void TakeDamage(int damage, IDamageable attacker);
    public void Knockback(Vector3 direction);
    public void ConnectGameStateManager();
    public void DisconnectGameStateManager();
}
