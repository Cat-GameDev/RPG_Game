
using UnityEngine;

public class ItemObject_Trigger : MonoBehaviour
{
    [SerializeField] ItemObject itemObject;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(Constants.PLAYER_TAG))
        {
            Player player = Cache.GetPlayer(other);
            if(player.characterStats.IsDead)
                return;

            itemObject.PickupItem();
            Inventory.Instance.UpdateSlotUI();
        }

        
        
    }
}
