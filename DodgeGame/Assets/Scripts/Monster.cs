using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private int _attackDamage;
    [SerializeField] private float _cooldown;
    [SerializeField] private float _rayShot;
    [SerializeField] private LayerMask _playerLayerMask;
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public int Health { get; private set; }

    private WaitForSeconds _waitRayShot;
    private WaitForSeconds _waitCooldown;
    private WaitUntil _waitUntilPlayerInTrigger;
    private WaitUntil _waitUntilPlayerInRange;
    private DetectionTrigger _detectionTrigger;
    private Transform _playerTransform => _detectionTrigger.TargetTransform;
    private bool _isPlayerInTrigger => _playerTransform != null;
    private bool _isPlayerInSight;
    public GameObject GameObject { get => gameObject; }

    private void Awake() => CacheComponents();
    private void Start()
    {
        Init();
        StartCoroutine(AttackRoutine());
        StartCoroutine(RayShotToTargetRoutine());
    }

    private void CacheComponents()
    {
        _detectionTrigger = GetComponentInChildren<DetectionTrigger>();

        _waitRayShot = new WaitForSeconds(_rayShot);
        _waitCooldown = new WaitForSeconds(_cooldown);
        _waitUntilPlayerInTrigger = new WaitUntil(() => _isPlayerInTrigger);
        _waitUntilPlayerInRange = new WaitUntil(() => _isPlayerInSight && _isPlayerInTrigger);
    }

    private IEnumerator RayShotToTargetRoutine()
    {
        while (true)
        {
            _isPlayerInSight = false;

            yield return _waitUntilPlayerInTrigger;
            RayShotToTarget();
            yield return _waitRayShot;
        }
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

        if (Physics.Raycast(ray, out hit, _detectionTrigger.Range, _playerLayerMask))
        {
            _isPlayerInSight = true;
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return _waitUntilPlayerInRange;
            Attack();
            yield return _waitCooldown;
        }
    }

    private void Attack()
    {
        IDamageable damageable = _playerTransform.GetComponent<IDamageable>();
        damageable?.TakeDamage(_attackDamage, this);
    }

    public void TakeDamage(int damage, IDamageable attacker)
    {
        Health -= damage;

        if (Health < 0)
        {
            Die();
            NotifyDeath(attacker);
        }
    }

    private void Die()
    {
        DisconnectGameStateManager();
        Destroy(gameObject);
    }

    public void Knockback(Vector3 direction)
    {
        GameObject.GetComponent<Rigidbody>()?.AddForce(direction, ForceMode.Impulse);
    }

    private void Init()
    {
        Health = MaxHealth;
        ConnectGameStateManager();
    }

    public void ConnectGameStateManager()
    {
        GameStateManager.Instance.AddDamageable(this);
    }

    public void DisconnectGameStateManager()
    {
        GameStateManager.Instance.RemoveDamageable(this);
    }

    private void NotifyDeath(IDamageable attacker)
    {
        GameStateManager.Instance.NotifyDeath(attacker, this);
    }
}
