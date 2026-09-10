using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour, IDamageable
{
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public int Health { get; private set; }

    [SerializeField] private GameManager _gameManager;
    [SerializeField] private LayerMask _playerLayerMask;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _cooldown;
    [SerializeField] private Transform _headTransform;
    [SerializeField] private Transform _muzzlePoint;

    [Header("Bullet")]
    [SerializeField] private BulletController _bulletPrefab;
    [SerializeField] private int _bulletDamage;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _bulletDestroyDelay;

    private float _currentCooldown;
    private Transform _playerTransform => _detectionTrigger.TargetTransform;
    private bool _isPlayerInTrigger => _playerTransform != null;
    private bool _isPlayerInSight = false;
    private bool _isReadyToFire => _currentCooldown >= _cooldown;
    private DetectionTrigger _detectionTrigger;
    public GameObject GameObject { get => gameObject; }

    private void Awake() => CacheComponents();
    private void Start() => Init();

    private void Update()
    {
        UpdateCurrentCooldown();
        RayShotToPlayer();
        Rotate();
        Fire();
    }

    private void CacheComponents()
    {
        _detectionTrigger = GetComponentInChildren<DetectionTrigger>();
    }

    private void Fire()
    {
        if (!_isPlayerInSight || !_isPlayerInTrigger) return;

        Vector3 look = new Vector3(
            _playerTransform.position.x,
            _headTransform.position.y,
            _playerTransform.position.z
            );

        _headTransform.LookAt(look);

        if (!_isReadyToFire) return;

        SpawnBullet();

        _currentCooldown = 0;
    }

    private void UpdateCurrentCooldown()
    {
        if (_isReadyToFire) return;

        _currentCooldown += Time.deltaTime;
    }

    private void SpawnBullet()
    {
        BulletController bullet = Instantiate(
            _bulletPrefab,
            _muzzlePoint.position,
            _muzzlePoint.rotation
            );

        bullet.SetData(_bulletDamage, _bulletSpeed, _bulletDestroyDelay, this);
    }

    private void Rotate()
    {
        if (_isPlayerInSight) return;

        _headTransform.Rotate(Vector3.up, _rotateSpeed * Time.deltaTime);
    }

    private void RayShotToPlayer()
    {
        _isPlayerInSight = false;
        if (!_isPlayerInTrigger) return;

        Vector3 from = new Vector3(
            transform.position.x,
            transform.position.y + _muzzlePoint.position.y,
            transform.position.z
            );

        Vector3 to = new Vector3(
            _playerTransform.position.x,
            _playerTransform.position.y + _muzzlePoint.position.y,
            _playerTransform.position.z);

        Ray ray = new Ray(from, (to - from).normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _detectionTrigger.Range, _playerLayerMask))
        {
            _isPlayerInSight = true;
        }
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
        DisconnectGameManager();
        Destroy(gameObject);
    }

    public void Knockback(Vector3 direction)
    {

    }

    private void Init()
    {
        Health = MaxHealth;
        ConnectGameManager();
    }

    public void ConnectGameManager()
    {
        _gameManager.AddDamageable(this);
    }

    public void DisconnectGameManager()
    {
        _gameManager.RemoveDamageable(this);
    }

    private void NotifyDeath(IDamageable attacker)
    {
        _gameManager.NotifyDeath(attacker, this);
    }
}
