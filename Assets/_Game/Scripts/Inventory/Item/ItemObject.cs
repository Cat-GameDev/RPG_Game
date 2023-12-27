
using UnityEngine;

public class ItemObject : GameUnit
{
    ItemData itemData;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Rigidbody2D rb;


    void OnInit()
    {
        if(itemData == null) return;

        sr.sprite = itemData.icon;
        gameObject.name = "Item Oject - " + itemData.name;
        Invoke(nameof(OnDespawn), 10f);
    }

    public void SetupItem(ItemData itemData, Vector2 velocity)
    {
        rb.velocity = velocity;
        this.itemData = itemData;
        OnInit();
    }

    public void PickupItem()
    {
        Inventory.Instance.AddItem(itemData);
        OnDespawn();
    }


}
