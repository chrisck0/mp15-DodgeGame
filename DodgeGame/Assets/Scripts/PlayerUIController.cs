using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _magazineText;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _grenadeText;

    private PlayerStat _stat;
    private PlayerWeapon _weapon;
    private PlayerGrenade _grenade;

    private void Awake() => CacheComponents();

    // TODO : Refactor using events
    private void Update()
    {
        RefreshMagazineUI();
        RefreshHealthUI();
        RefreshGrenadeUI();
    }

    private void CacheComponents()
    {
        _stat = GetComponent<PlayerStat>();
        _weapon = GetComponentInChildren<PlayerWeapon>();
        _grenade = GetComponentInChildren<PlayerGrenade>();
    }

    private void RefreshMagazineUI()
    {
        _magazineText.text = $"{_weapon.CurrentMagazine} / {_weapon.MaxMagazine}";
    }

    private void RefreshHealthUI()
    {
        _healthText.text = $"HP : {_stat.Health}";
    }

    private void RefreshGrenadeUI()
    {
        _grenadeText.text = $"{_grenade.CurrentGrenadeNumber} / {_grenade.MaxGrenadeNumber}";
    }
}
