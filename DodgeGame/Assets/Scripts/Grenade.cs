using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    // 설정한 딜레이 후 자동으로 꺼지게 하기
    [SerializeField] private float _destroyDelay;
    [SerializeField] private FlameEffect _grenadeExplosionEffect;
    [SerializeField] private float _explosionRange;
    [SerializeField] private int _explosionDamage;
    private float _elapsedTime;
    
    // --------------------------------------------------
    private void OnEnable() => ResetElapsedTime();
    private void Update()
    {
        UpdateElapsedTime();
        ChangeState();
    }
    // --------------------------------------------------

    public void Play()
    {
        ResetElapsedTime();
    }

    private void ResetElapsedTime()
    {
        _elapsedTime = 0;
    }

    private void UpdateElapsedTime()
    {
        _elapsedTime += Time.deltaTime;
    }

    private void ChangeState()
    {
        if (_elapsedTime < _destroyDelay) return;

        Explode();
        Destroy(gameObject);
        Instantiate(_grenadeExplosionEffect, transform.position, transform.rotation);
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRange);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(_explosionDamage);
            }
        }
    }
}