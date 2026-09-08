using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    [SerializeField] private int _attackDamage;
    [SerializeField] private float _cooldown;

    private SphereCollider _sphereCollider;
    private Transform _playerTransform;
    private float _currentCooldown;
    private bool _isPlayerInTrigger => _playerTransform != null;
    private bool _isPlayerInSight;
    private bool _isReadyToAttack => _currentCooldown >= _cooldown;
    public GameObject GameObject { get => gameObject; }

    private void Awake() => CacheComponents();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            _playerTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) {
            _playerTransform = null;
        }
    }

    private void Update()
    {
        UpdateCurrentCooldown();
        RayShotToTarget();
        Attack();
    }

    private void CacheComponents()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }

    private void UpdateCurrentCooldown()
    {
        if (_isReadyToAttack) return;

        _currentCooldown += Time.deltaTime;
    }

    private void RayShotToTarget()
    {
        _isPlayerInSight = false;
        if (!_isPlayerInTrigger) return;

        Vector3 from = transform.position;
        Vector3 to = new Vector3(
            _playerTransform.position.x,
            transform.position.y,
            _playerTransform.position.z
            );

        Ray ray = new Ray(transform.position, (to - from).normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _sphereCollider.radius))
        {
            if (hit.transform.CompareTag("Player"))
            {
                _isPlayerInSight = true;
            }
        }
    }

    private void Attack()
    {
        if (!_isPlayerInSight || !_isPlayerInTrigger || !_isReadyToAttack) return;

        Debug.Log($"{gameObject.name}가 플레이어 공격");

        IDamageable damageable = _playerTransform.GetComponent<IDamageable>();
        damageable?.TakeDamage(_attackDamage);

        _currentCooldown = 0;
    }


    public void TakeDamage(int damage)
    {
        Debug.Log($"{gameObject.name}가 데미지 {damage} 입음");
    }
}
