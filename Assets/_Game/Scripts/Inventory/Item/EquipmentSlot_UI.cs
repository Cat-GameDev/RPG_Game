
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot_UI : ItemSlot_UI
{
    public EquipmentType equipmentType;

    void OnValidate()
    {
        gameObject.name = "Equipment slot - " + equipmentType.ToString();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if(item == null || item.itemData == null) return;
        
        Inventory.Instance.UnequipItem(item.itemData as ItemData_Equip);
        Inventory.Instance.AddItem(item.itemData as ItemData_Equip);
        ClearUpSlot();
    }
}
