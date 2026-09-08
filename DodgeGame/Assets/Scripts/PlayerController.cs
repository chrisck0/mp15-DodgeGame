using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IInteractor, IDamageable
{
    [SerializeField] private Transform _cameraPivot;
    [SerializeField] private float _detectionRange;
    [SerializeField] private KeyCode _interactionKey = KeyCode.E;
    [SerializeField] private int _health;

    private PlayerGrenade _grenade;
    private PlayerWeapon _weapon;
    private PlayerMovement _movement;
    private Transform _cameraTransform;
    private SteamPack _steamPack;
    private float _elapsedBuffTime;
    private IInteractable _targetInteractable;

    private bool _hasDetectInteractable => _targetInteractable != null;
    private bool _isPressedInteractionKey => Input.GetKeyDown(_interactionKey);
    private bool _canInteraction => _hasDetectInteractable && _isPressedInteractionKey;
    private bool _hasSteamPack => _steamPack != null;

    public GameObject GameObject { get => gameObject; }
    
    // ----------------------------------------------
    private void Awake() => CacheComponents();
    private void Start() => LockCursor();
    private void FixedUpdate() => _movement.Move();
    // ----------------------------------------------

    private void Update()
    {
        _movement.Rotate();
        _weapon.Reload();
        _weapon.Fire();
        DetectInteractable();
        TryInteract();
        ChangeStat();
    }

    private void LateUpdate()
    {
        SetWeaponTransform();
        SetCameraTransform();
    }

    private void CacheComponents()
    {
        _movement = GetComponent<PlayerMovement>();
        _weapon = GetComponentInChildren<PlayerWeapon>();
        _grenade = GetComponentInChildren<PlayerGrenade>();
        _cameraTransform = Camera.main.transform;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SetWeaponTransform()
    {
        _weapon.transform.SetPositionAndRotation(
            _cameraPivot.position,
            _cameraPivot.rotation
            );
    }

    private void SetCameraTransform()
    {
        _cameraTransform.SetPositionAndRotation(
            _cameraPivot.position,
            _cameraPivot.rotation
            );
    }

    public void DetectInteractable()
    {
        Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, _detectionRange))
        {
            if (_hasDetectInteractable)
            {
                _targetInteractable.Untargeting();
                _targetInteractable = null;
            }

            return;
        }

        if (_hasDetectInteractable)
        {
            if (hit.collider.gameObject == _targetInteractable.GameObject)
            {
                return; // 같은 Interactable을 계속 주시하고 있는 경우
            }
        }

        _targetInteractable?.Untargeting();
        _targetInteractable = hit.collider.GetComponent<IInteractable>();

        _targetInteractable?.Targeting();
    }

    public void TryInteract()
    {
        if (!_canInteraction) return;

        _targetInteractable.Interact(this);
        _targetInteractable = null;
    }

    public void SetSteamPack()
    {
        _steamPack = gameObject.AddComponent<SteamPack>();
        DecreaseHealth(_steamPack.GetHealthDecrease());
        _movement.AddSpeed(_steamPack.GetMoveSpeedIncrease());
        _weapon.DecreaseCooldown(_steamPack.GetCooldownDecrease());
    }

    private void ChangeStat()
    {
        if (!_hasSteamPack) return;

        _elapsedBuffTime += Time.deltaTime;

        if (_elapsedBuffTime > _steamPack.GetBuffTime())
        {
            _movement.AddSpeed(-_steamPack.GetMoveSpeedIncrease());
            _weapon.DecreaseCooldown(-_steamPack.GetCooldownDecrease());
            Destroy(_steamPack);
            _steamPack = null;
        }
    }

    private void DecreaseHealth(int damage)
    {
        if (_health <= 0) return;
        _health -= damage;
    }

    public void TakeDamage(int damage)
    {
        if (_health <= 0) return;
        _health -= damage;
        Debug.Log($"{gameObject.name}가 데미지 {damage} 입음");
    }
}
