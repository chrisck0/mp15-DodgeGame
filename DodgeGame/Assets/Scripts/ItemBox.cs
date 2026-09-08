using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemBox : MonoBehaviour, IInteractable, IDamageable
{
    public GameObject GameObject { get => gameObject; }
    private Outline _outline;

    private void Awake() => CacheComponents();
    private void Start() => Init();

    public void Targeting()
    {
        _outline.enabled = true;
    }

    public void Untargeting()
    {
        _outline.enabled = false;
    }

    public void Interact(IInteractor owner)
    {
        if (!(owner is PlayerController)) return;

        PlayerController player = (PlayerController)owner;

        Debug.Log("ItemBox : Player Interacted!");
    
        player.SetSteamPack();

        // owner의 능력치 상승
        // 인벤토리로 들어감
        // 무기가 생김
        // 장탄수 리필
        // ...

        // 이동속도 변화 ()
        // 버프 같은 걸 객체로 만들 수 있다
        Destroy(gameObject);
    }

    private void Init()
    {
        _outline.enabled = false;
    }

    private void CacheComponents()
    {
        _outline = gameObject.GetComponent<Outline>();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"{gameObject.name}이 데미지 {damage} 입음");
    }
}
