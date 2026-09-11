using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour, IDamageable
{
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public int Health { get; private set; }
    
    [SerializeField] private ObjectPool _bulletPool;
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
    [SerializeField] private float _returnDelay;

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
        // 1. 얻어오기
        IPoolable bullet = _bulletPool.Take();

        if (bullet == null) return;

        // 2. Transform.position, rotation 설정
        bullet.tr.position = _muzzlePoint.position;
        bullet.tr.rotation = _muzzlePoint.rotation;

        // 3. 활성화
        bullet.tr.gameObject.SetActive(true);

        // Getcomponent보다 casting을 사용하자 (연산 자체가 더 적다)
        (bullet as BulletController).SetData(_bulletDamage, _bulletSpeed, _returnDelay, this);
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
        DisconnectGameStateManager();
        Destroy(gameObject);
    }

    public void Knockback(Vector3 direction)
    {

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
