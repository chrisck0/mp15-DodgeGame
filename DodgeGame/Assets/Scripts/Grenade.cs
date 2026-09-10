using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    // 설정한 딜레이 후 자동으로 꺼지게 하기
    [SerializeField] private float _destroyDelay;
    [SerializeField] private GameObject _grenadeBody;
    [SerializeField] private GameObject _grenadeExplosionEffect;
    [SerializeField] private float _explosionRange;
    [SerializeField] private int _explosionDamage;
    [SerializeField] private float _knockbackForce;
    private float _elapsedTime;
    private PlayerController _owner;

    // --------------------------------------------------
    private void Start() => Init();
    private void OnEnable() => ResetElapsedTime();
    private void Update()
    {
        UpdateElapsedTime();
        Explode();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRange);
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

    private void Explode()
    {
        if (_elapsedTime < _destroyDelay) return;

        _grenadeBody.SetActive(false);
        _grenadeExplosionEffect.SetActive(true);

        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRange);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(_explosionDamage, _owner);
                damageable.Knockback(GetKnockbackDirection(collider.transform));
            }
        }

        Destroy(gameObject, 1f);
    }

    private Vector3 GetKnockbackDirection(Transform target)
    {
        Vector3 dir = (target.position - transform.position) * _knockbackForce;
        dir.y = Random.Range(0f, 5f);

        return dir;
    }

    public void SetOwner(PlayerController owner)
    {
        _owner = owner;
    }

    private void Init()
    {
        _grenadeExplosionEffect.SetActive(false);
    }
}