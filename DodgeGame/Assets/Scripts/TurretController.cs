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
    [SerializeField] private float _rayShot;
    [SerializeField] private Transform _headTransform;
    [SerializeField] private Transform _muzzlePoint;

    [Header("Bullet")]
    [SerializeField] private BulletController _bulletPrefab;
    [SerializeField] private int _bulletDamage;
    [SerializeField] private float _bulletSpeed;

    private WaitForSeconds _waitRayShot;
    private WaitForSeconds _waitCooldown;
    private WaitUntil _waitUntilPlayerInTrigger;
    private WaitUntil _waitUntilPlayerInRange;
    private Transform _playerTransform => _detectionTrigger.TargetTransform;
    private DetectionTrigger _detectionTrigger;
    private bool _isPlayerInTrigger => _playerTransform != null;
    private bool _isPlayerInSight = false;
    private bool _hasFired = false;
    public GameObject GameObject { get => gameObject; }

    private void Awake() => CacheComponents();
    private void Start()
    {
        Init();
        StartCoroutine(RayShotToPlayerRoutine());
        StartCoroutine(FireRoutine());
    }

    private void Update()
    {
        Rotate();
    }

    private void CacheComponents()
    {
        _detectionTrigger = GetComponentInChildren<DetectionTrigger>();
        _waitRayShot = new WaitForSeconds(_rayShot);
        _waitCooldown = new WaitForSeconds(_cooldown);
        _waitUntilPlayerInTrigger = new WaitUntil(() => _isPlayerInTrigger);
        _waitUntilPlayerInRange = new WaitUntil(() => _isPlayerInSight && _isPlayerInTrigger);
    }

    private void LookAt()
    {
        if (!_isPlayerInSight || !_isPlayerInTrigger) return;

        Vector3 look = new Vector3(
            _playerTransform.position.x,
            _headTransform.position.y,
            _playerTransform.position.z
            );

        _headTransform.LookAt(look);
    }

    private IEnumerator RayShotToPlayerRoutine()
    {
        while (true)
        {
            _isPlayerInSight = false;

            yield return _waitUntilPlayerInTrigger;
            RayShotToPlayer();
            yield return _waitRayShot;
        }
    }

    private IEnumerator FireRoutine()
    {
        while (true)
        {
            yield return _waitUntilPlayerInRange;
            LookAt();
            if (_hasFired == false) StartCoroutine(SpawnBulletRoutine());
        }
    }

    private IEnumerator SpawnBulletRoutine()
    {
        SpawnBullet();
        _hasFired = true;
        yield return _waitCooldown;
        _hasFired = false;
    }

    private void SpawnBullet()
    {
        // 1. 얻어오기
        IPoolable bullet = _bulletPool.Take();

        if (bullet == null) return;

        // 2. Transform.position, rotation 설정
        bullet.tr.position = _muzzlePoint.position;
        bullet.tr.rotation = _muzzlePoint.rotation;

        // Getcomponent보다 casting을 사용하자 (연산 자체가 더 적다)
        (bullet as BulletController).SetData(_bulletDamage, _bulletSpeed, this);

        // 3. 활성화
        bullet.tr.gameObject.SetActive(true);
    }

    private void Rotate()
    {
        if (_isPlayerInSight) return;

        _headTransform.Rotate(Vector3.up, _rotateSpeed * Time.deltaTime);
    }

    private void RayShotToPlayer()
    {
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
