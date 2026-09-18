using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageableUIController : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private Image _healthImage;

    private Transform _cameraTransform;
    private IDamageable _damageable;

    private void Awake() => CacheComponents();
    private void Update() => UpdateHealthUI();

    private void CacheComponents()
    {
        _damageable = GetComponent<IDamageable>();
        _cameraTransform = Camera.main.transform;
    }

    private void UpdateHealthUI()
    {
        float healthRate = (float)_damageable.Health / _damageable.MaxHealth;
        _healthImage.transform.localScale = new Vector3(
            healthRate,
            _healthImage.transform.localScale.y,
            _healthImage.transform.localScale.z
            );

        _healthText.text = $"{_damageable.Health} / {_damageable.MaxHealth}";

        _canvas.transform.LookAt(_cameraTransform);
        _canvas.transform.Rotate(transform.up, 180f);
    }
}
