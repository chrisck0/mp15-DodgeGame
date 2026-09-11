using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemBox : MonoBehaviour, IInteractable
{
    [SerializeField] private StimPack _stimPackPrefab;
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

        Instantiate(_stimPackPrefab, player.transform)
            .SetPlayerController(player)
            .SetMoveSpeed(10f)
            .SetCooldown(0.1f)
            .SetDamage(20)
            .Activate();

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
}
