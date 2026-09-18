using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator _animator;

    private void Awake() => CacheComponents();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetBool("IsOpen", true);
            // _animator.SetInteger(파라미터 이름, 값);
            // _animator.SetFloat(파라미터 이름, 값);
            // _animator.SetTrigger(파라미터 이름);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetBool("IsOpen", false);
        }
    }

    private void CacheComponents()
    {
        _animator = GetComponent<Animator>();
    }
}
