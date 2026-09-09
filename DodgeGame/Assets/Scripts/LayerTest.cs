using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerTest : MonoBehaviour
{
    public LayerMask TargetLayer;
    public float Range;

    private void OnTriggerEnter(Collider other)
    {
        if (TargetLayer.Contains(other))
        {
            Debug.Log("찾음");
        }
        Debug.Log(other.gameObject.layer);
    }
}
