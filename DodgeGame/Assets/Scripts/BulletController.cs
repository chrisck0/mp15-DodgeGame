using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour, IPoolable
{
    [SerializeField] private LayerMask _playerLayerMask;
    [SerializeField] private float _returnDelay;

    private TurretController _owner;
    private int _damage;
    private float _speed;
    private WaitForSeconds _wait;

    public ObjectPool Pool { get; set; }
    public Transform tr { get => transform; }

    // ------------------------
    private void Awake() => CacheComponents();
    private void OnEnable() => StartCoroutine(ReturnToPoolRoutine());

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

        Pool.Return(this);
    }

    private void Update() => MoveForward();
    // ------------------------

    private void CacheComponents()
    {
        _wait = new WaitForSeconds(_returnDelay);
    }

    // 앞으로 전진
    private void MoveForward()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    // 터렛으로부터 데이터 전달 받기
    public void SetData(int damage, float speed, TurretController owner)
    {
        _damage = damage;
        _speed = speed;
        _owner = owner;
    }

    public IEnumerator ReturnToPoolRoutine()
    {
        yield return _wait;
        Pool.Return(this);
    }
}
