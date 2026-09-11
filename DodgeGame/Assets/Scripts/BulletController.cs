using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour, IPoolable
{
    [SerializeField] private LayerMask _playerLayerMask;
    private TurretController _owner;
    private int _damage;
    private float _speed;
    private float _returnDelay;
    private float _elapsedTime;

    public ObjectPool Pool { get; set; }
    public Transform tr { get => transform; }

    // 어딘가에 부딪히면
    private void OnTriggerEnter(Collider other)
    {
        if (_playerLayerMask.Contains(other))
        {
            if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController))
            {
                playerController.TakeDamage(_damage, _owner);
            }
        }

        _elapsedTime = 0;
        Pool.Return(this);
    }

    private void Update()
    {
        UpdateElapsedTime();
        MoveForward();
        ReturnToPool();
    }

    // 앞으로 전진
    private void MoveForward()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    // 터렛으로부터 데이터 전달 받기
    public void SetData(int damage, float speed, float returnDelay, TurretController owner)
    {
        _damage = damage;
        _speed = speed;
        _returnDelay = returnDelay;
        _owner = owner;
    }

    private void UpdateElapsedTime()
    {
        _elapsedTime += Time.deltaTime;
    }

    public void ReturnToPool()
    {
        if (_elapsedTime >= _returnDelay)
        {
            _elapsedTime = 0;
            Pool.Return(this);
        }
    }
}
