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
    private float _elapsedTime;
    private bool _isBuffFinished => _elapsedTime >= _buffTime;
    private PlayerController _playerController;

    private void Awake()
    {
        _elapsedTime = 0f;
    }

    private void Update()
    {
        UpdateElapsedTime();
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

    public void Activate()
    {
        _originalMoveSpeed = _playerController.Stat.MoveSpeed;
        _originalCooldown = _playerController.Stat.WeaponCooldown;

        _playerController.Stat.MoveSpeed = _moveSpeed;
        _playerController.Stat.WeaponCooldown = _cooldown;
        _playerController.Stat.Health -= _damage;
    }

    private void Deactivate()
    {
        if (!_isBuffFinished) return;

        _playerController.Stat.MoveSpeed = _originalMoveSpeed;
        _playerController.Stat.WeaponCooldown = _originalCooldown;
        Destroy(gameObject);
    }

    private void UpdateElapsedTime()
    {
        _elapsedTime += Time.deltaTime;
    }
}