using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StimPack : MonoBehaviour
{
    private float _moveSpeed;
    private float _cooldown;
    private int _damage;

    private float _originalMoveSpeed;
    private float _originalCooldown;

    private float _buffTime = 20f;
    private PlayerController _playerController;

    private void Start() => StartCoroutine(StimPackRoutine());

    private IEnumerator StimPackRoutine()
    {
        Activate();
        yield return new WaitForSeconds(_buffTime);
        Deactivate();
    }

    public StimPack SetMoveSpeed(float moveSpeed)
    {
        _moveSpeed = moveSpeed;
        return this;
    }

    public StimPack SetCooldown(float cooldown)
    {
        _cooldown = cooldown;
        return this;
    }

    public StimPack SetDamage(int damage)
    {
        _damage = damage;
        return this;
    }

    public StimPack SetPlayerController(PlayerController playerController)
    {
        _playerController = playerController;
        return this;
    }

    private void Activate()
    {
        _originalMoveSpeed = _playerController.Stat.MoveSpeed;
        _originalCooldown = _playerController.Stat.WeaponCooldown;

        _playerController.Stat.MoveSpeed = _moveSpeed;
        _playerController.Stat.WeaponCooldown = _cooldown;
        _playerController.Stat.Health -= _damage;
    }

    private void Deactivate()
    {
        _playerController.Stat.MoveSpeed = _originalMoveSpeed;
        _playerController.Stat.WeaponCooldown = _originalCooldown;
        Destroy(gameObject);
    }
}