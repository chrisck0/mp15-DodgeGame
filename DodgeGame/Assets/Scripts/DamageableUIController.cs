using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageableUIController : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
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
        _canvas.transform.LookAt(_cameraTransform);
    }
}
