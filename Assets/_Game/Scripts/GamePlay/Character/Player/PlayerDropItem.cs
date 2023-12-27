using System.Collections.Generic;
using UnityEngine;

public class PlayerDropItem : ItemDrop
{
    [Range(0, 100)]
    public float chanceToLooseItem;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.Instance;
        List<InventoryItem> currentEquipment = inventory.GetEquipmentList();
        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();

        for (int i = 0; i < currentEquipment.Count; i++)
        {
            if(Random.Range(1, 100) < chanceToLooseItem)
            {
                Dropitem(currentEquipment[i].itemData);
                itemsToUnequip.Add(currentEquipment[i]);

            }
        }

        if(itemsToUnequip.Count > 0)
        {
            for (int i = 0; i < itemsToUnequip.Count; i++)
            {
                inventory.UnequipItem(itemsToUnequip[i].itemData as ItemData_Equip);
            }
        }
        
    }
}