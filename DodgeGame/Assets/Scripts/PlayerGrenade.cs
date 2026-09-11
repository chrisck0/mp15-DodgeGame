using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGrenade : MonoBehaviour
{
    [SerializeField] private float _maxChargePower;
    [SerializeField] private KeyCode _grenadeKey = KeyCode.Alpha3;
    [SerializeField] private Grenade _grenadePrefab;
    [SerializeField] private float _chargePowerScale;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private int _maxGrenadeNumber;

    public int MaxGrenadeNumber => _maxGrenadeNumber;
    public int CurrentGrenadeNumber => _currentGrenadeNumber;

    private PlayerController _controller;
    private float _chargePower;
    private int _currentGrenadeNumber;

    private bool _isPressingGrenadeKey => Input.GetKey(_grenadeKey);
    private bool _hasChargePower => _chargePower != 0;
    private bool _hasMaxChargePower => _chargePower >= _maxChargePower;

    private void Awake() => CacheComponents();
    private void Start() => Init();
    private void Update() => ChargePower();

    private void CacheComponents()
    {
        _controller = GetComponentInParent<PlayerController>();
    }

    private void Init()
    {
        ResetChargePower();
        _currentGrenadeNumber = _maxGrenadeNumber;
    }

    private void ResetChargePower()
    {
        _chargePower = 0f;
    }
    private void ChargePower()
    {
        if (!_isPressingGrenadeKey && !_hasChargePower) return;

        if (_isPressingGrenadeKey && !_hasMaxChargePower)
        {
            _chargePower += Time.deltaTime;
        }
        else
        {
            if (_isPressingGrenadeKey) return;

            if (_hasMaxChargePower) _chargePower = _maxChargePower;
            ThrowGrenade(_chargePower);
            ResetChargePower();
        }
    }

    private void ThrowGrenade(float chargePower)
    {
        if (_currentGrenadeNumber <= 0) return;

        Grenade grenade = Instantiate(_grenadePrefab, transform.position, transform.rotation);
        grenade.SetOwner(_controller);
        Rigidbody grenadeRigidbody = grenade.GetComponent<Rigidbody>();

        grenadeRigidbody.AddForce(
            playerTransform.forward.x * chargePower * _chargePowerScale,
            transform.forward.y * chargePower * _chargePowerScale,
            playerTransform.forward.z * chargePower * _chargePowerScale);

        _currentGrenadeNumber--;
        // Debug.Log($"PlayerGrenade : 힘 {chargePower}만큼 수류탄 투척, 수류탄 {_currentGrenadeNumber}개 남음");
    }
}
