using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayerMask;
    private int _damage;
    private float _speed;

    // 어딘가에 부딪히면
    private void OnTriggerEnter(Collider other)
    {
        if (_playerLayerMask.Contains(other))
        {
            if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController))
            {
                playerController.TakeDamage(_damage);
            }
        }
        Debug.Log($"Collider is : {other.name}, Destroy bullet!");
        Destroy(gameObject);
    }

    private void Update() => MoveForward();

    // 앞으로 전진
    private void MoveForward()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    // 터렛으로부터 데이터 전달 받기
    public void SetData(int damage, float speed, float destroyDelay)
    {
        _damage = damage;
        _speed = speed;

        Destroy(gameObject, destroyDelay);
    }
}
