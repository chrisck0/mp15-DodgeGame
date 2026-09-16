using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIBinder : MonoBehaviour
{
    [SerializeField] private PlayerUIController _playerUIController;

    private PlayerStat _stat;
    private PlayerWeapon _weapon;
    private PlayerGrenade _grenade;

    private bool _isGameStateManagerEventsBinded;

    // ------------------
    private void Awake() => CacheComponents();
    private void Start()
    {
        if (_isGameStateManagerEventsBinded) return;

        BindGameStateManagerEvents();
    }

    private void OnEnable()
    {
        if (GameStateManager.Instance != null)
        {
            BindGameStateManagerEvents();
        }

        BindPlayerStatChangeEvents();
    }

    private void OnDisable()
    {
        UnbindPlayerStatChangeEvents();
        UnbindGameStateManagerEvents();
    }
    // ------------------

    private void CacheComponents()
    {
        _stat = GetComponent<PlayerStat>();
        _weapon = GetComponentInChildren<PlayerWeapon>();
        _grenade = GetComponentInChildren<PlayerGrenade>();
    }

    private void BindPlayerStatChangeEvents()
    {
        _stat.OnHealthChanged += _playerUIController.RefreshHealthUI;
        _weapon.OnCurrentMagazineChanged += _playerUIController.RefreshMagazineUI;
        _grenade.OnCurrentGrenadeNumberChanged += _playerUIController.RefreshGrenadeUI;
    }

    private void UnbindPlayerStatChangeEvents()
    {
        _stat.OnHealthChanged -= _playerUIController.RefreshHealthUI;
        _weapon.OnCurrentMagazineChanged -= _playerUIController.RefreshMagazineUI;
        _grenade.OnCurrentGrenadeNumberChanged -= _playerUIController.RefreshGrenadeUI;
    }

    private void BindGameStateManagerEvents()
    {
        GameStateManager.Instance.OnDamageableChanged += _playerUIController.RefreshKillLog;
        _isGameStateManagerEventsBinded = true;
    }

    private void UnbindGameStateManagerEvents()
    {
        GameStateManager.Instance.OnDamageableChanged -= _playerUIController.RefreshKillLog;
        _isGameStateManagerEventsBinded = false;
    }
}
