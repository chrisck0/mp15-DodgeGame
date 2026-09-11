using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [field: SerializeField] public int Size { get; private set; }
    private IPoolable[] _pool;
    public int Count { get; private set; }

    public bool IsEmpty => Count == 0;

    private void Awake() => Init();

    public IPoolable Take()
    {
        if (IsEmpty) return null;

        Count--;
        IPoolable poolable = _pool[Count];
        // 여기서 활성화시켜도 상관 없음.
        _pool[Count] = null;

        return poolable;
    }

    public void Return(IPoolable poolable)
    {
        if (Size <= Count) return;

        _pool[Count] = poolable;
        poolable.tr.gameObject.SetActive(false);
        Count++;
    }

    private void Init()
    {
        _pool = new IPoolable[Size];

        for (int i = 0; i < _pool.Length; i++)
        {
            GameObject gameObject = Instantiate(_prefab);
            _pool[i] = gameObject.GetComponent<IPoolable>();
            _pool[i].Pool = this;
            gameObject.SetActive(false);
        }

        Count = Size;
    }
}
