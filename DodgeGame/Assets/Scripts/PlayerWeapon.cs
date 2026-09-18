using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerWeapon : MonoBehaviour
{
    private Transform _cameraTransform;

    [SerializeField] private KeyCode _fireKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode _reloadKey = KeyCode.R;
    [SerializeField] private float _reloadDelay;
    [SerializeField] private int _maxMagazine;
    [SerializeField] private FlameEffect _flameEffect;
    [SerializeField] private FlameEffect _bulletImpactEffectPrefab;
    [SerializeField] private LayerMask _targetLayerMask;

    private WaitForSeconds _waitReload;
    private WaitForSeconds _waitCooldown;
    private WaitUntil _waitCanFire;
    public event Action<int, int> OnCurrentMagazineChanged;
    private PlayerController _controller;
    private PlayerStat _stat;
    private float _range => _stat.WeaponRange;
    private int _damage => _stat.Damage;
    private float _cooldown => _stat.WeaponCooldown;

    private int _currentMagazine;
    public int CurrentMagazine
    {
        get => _currentMagazine;
        set
        {
            _currentMagazine = value;
            OnCurrentMagazineChanged?.Invoke(value, _maxMagazine);
        }
    }

    private bool _isReloading;
    private bool _isPressedFire => Input.GetKey(_fireKey);
    private bool _isPressedReload => Input.GetKeyDown(_reloadKey);
    private bool _hasBullets => _currentMagazine > 0;
    private bool _canFire => _isPressedFire && _hasBullets && !_isReloading;

    // ------------------------------------------------
    private void Awake() => CacheComponents();

    private void Start()
    {
        Init();
        StartCoroutine(FireRoutine());
    }
    // ------------------------------------------------

    public void DecreaseCooldown(float cooldown)
    {
        _stat.WeaponCooldown -= cooldown;
    }

    public void Reload()
    {
        // 3초 대기
        if (!_isPressedReload || _isReloading) return;
        StartCoroutine(ReloadRoutine());
    }
    
    public IEnumerator ReloadRoutine()
    {
        _isReloading = true;
        yield return _waitReload;
        CurrentMagazine = _maxMagazine;
        _isReloading = false;
    }

    private void PlayFlameEffect()
    {
        _flameEffect.gameObject.SetActive(true);
        _flameEffect.Play();
    }

    private void PlayBulletImpactEffect(RaycastHit hit)
    {
        Transform effectTransform = Instantiate(_bulletImpactEffectPrefab).transform;
        effectTransform.position = hit.point;
        effectTransform.forward = hit.normal;
    }

    private IEnumerator FireRoutine()
    {
        while (true)
        {
            yield return _waitCanFire;
            Fire();
            yield return _waitCooldown;
        }
    }

    public void Fire()
    {
        if (!_canFire) return;
        CurrentMagazine--;
        PlayFlameEffect();

        if (!TryGetDamageable(out IDamageable damageable)) return;

        damageable.TakeDamage(_damage, _controller);
    }

    private bool TryGetDamageable(out IDamageable damageable)
    {
        bool result = false;
        damageable = null;

        Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _range, _targetLayerMask))
        {
            PlayBulletImpactEffect(hit);
            result = hit.transform.TryGetComponent<IDamageable>(out damageable);
        }

        return result;
    }

    private void CacheComponents()
    {
        _cameraTransform = Camera.main.transform;
        _stat = GetComponentInParent<PlayerStat>();
        _controller = GetComponentInParent<PlayerController>();

        _waitReload = new WaitForSeconds(_reloadDelay);
        _waitCooldown = new WaitForSeconds(_cooldown);
        _waitCanFire = new WaitUntil(() => _canFire);
}

    private void Init()
    {
        _currentMagazine = _maxMagazine;
    }
}
