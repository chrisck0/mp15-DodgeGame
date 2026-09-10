using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class DetectionTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayerMask;

    private SphereCollider _sphereCollider;
    public Transform TargetTransform { get; private set; }
    public float Range => _sphereCollider.radius;

    private void Awake() => CacheComponents();
    private void Start() => Init();

    private void OnTriggerEnter(Collider other)
    {
        if (_targetLayerMask.Contains(other))
        {
            TargetTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_targetLayerMask.Contains(other))
        {
            TargetTransform = null;
        }
    }

    private void CacheComponents()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }

    private void Init()
    {
        TargetTransform = null;
    }
}
