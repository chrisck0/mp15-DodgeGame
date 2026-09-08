using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    [SerializeField] private int _attackDamage;

    private SphereCollider _sphereCollider;
    private Transform _playerTransform;
    private bool _isPlayerInTrigger => _playerTransform != null;
    private bool _isPlayerInSight;
    public GameObject GameObject { get => gameObject; }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerTransform = null;
        }
    }

    private void Awake() => CacheComponents();
    private void Update()
    {
        RayShotToTarget();
    }

    private void CacheComponents()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }

    private void RayShotToTarget()
    {
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
                IDamageable damageable = hit.transform.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    Attack(damageable);
                    _isPlayerInSight = true;
                }
            }
        }

    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"{gameObject.name}이 데미지 {damage} 입음");
    }

    private void Attack(IDamageable damageable)
    {
        Debug.Log($"{gameObject.name}이 공격");
        damageable.TakeDamage(_attackDamage);
    }
}
