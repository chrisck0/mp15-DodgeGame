using UnityEngine;

public static class LayerMaskExtensions
{
    public static bool Contains(this LayerMask mask, int layer)
    {
        return (mask.value & (1 << layer)) != 0;
    }

    public static bool Contains(this LayerMask mask, GameObject gameObject)
    {
        return (mask.value & (1 << gameObject.layer)) != 0;
    }

    public static bool Contains(this LayerMask mask, Collider component)
    {
        return (mask.value & (1 << component.gameObject.layer)) != 0;
    }

    public static LayerMask Add(this LayerMask mask, int layer)
    {
        return (mask.value | (1 << layer));
    }

    public static LayerMask Add(this LayerMask mask, GameObject gameObject)
    {
        return (mask.value | (1 << gameObject.layer));
    }

    public static LayerMask Add(this LayerMask mask, Collider component)
    {
        return (mask.value | (1 << component.gameObject.layer));
    }

    public static LayerMask Remove(this LayerMask mask, int layer)
    {
        return (mask.value &  ~(1 << layer));
    }

    public static LayerMask Remove(this LayerMask mask, GameObject gameObject)
    {
        return (mask.value &  ~(1 << gameObject.layer));
    }

    public static LayerMask Remove(this LayerMask mask, Collider component)
    {
        return (mask.value &  ~(1 << component.gameObject.layer));
    }

    public static LayerMask Everything(this LayerMask mask)
    {
        return ~0;
    }

    public static LayerMask Nothing(this LayerMask mask)
    {
        return 0;
    }
}
