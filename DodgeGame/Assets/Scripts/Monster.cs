using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private int _attackDamage;
    [SerializeField] private float _cooldown;
    [SerializeField] private LayerMask _playerLayerMask;
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public int Health { get; private set; }

    private DetectionTrigger _detectionTrigger;
    private Transform _playerTransform => _detectionTrigger.TargetTransform;
    private float _currentCooldown;
    private bool _isPlayerInTrigger => _playerTransform != null;
    private bool _isPlayerInSight;
    private bool _isReadyToAttack => _currentCooldown >= _cooldown;
    public GameObject GameObject { get => gameObject; }

    private void Awake() => CacheComponents();
    private void Start() => Init();

    private void Update()
    {
        UpdateCurrentCooldown();
        RayShotToTarget();
        Attack();
    }

    private void CacheComponents()
    {
        _detectionTrigger = GetComponentInChildren<DetectionTrigger>();
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

        if (Physics.Raycast(ray, out hit, _detectionTrigger.Range, _playerLayerMask))
        {
            _isPlayerInSight = true;
        }
    }

    private void Attack()
    {
        if (!_isPlayerInSight || !_isPlayerInTrigger || !_isReadyToAttack) return;

        IDamageable damageable = _playerTransform.GetComponent<IDamageable>();
        damageable?.TakeDamage(_attackDamage, this);

        _currentCooldown = 0;
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
