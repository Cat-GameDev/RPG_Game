using System;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : Singleton<Inventory>
{
    public List<ItemData> staringItem = new List<ItemData>();
    public List<InventoryItem> inventory = new List<InventoryItem>();
    public Dictionary<ItemData, InventoryItem> inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

    public List<InventoryItem> stash = new List<InventoryItem>();
    public Dictionary<ItemData, InventoryItem> stashDictionary = new Dictionary<ItemData, InventoryItem>();

    public List<InventoryItem> equipment = new List<InventoryItem>();
    public Dictionary<ItemData_Equip, InventoryItem> equipmentDictionary = new Dictionary<ItemData_Equip, InventoryItem>();

    [Header("Inventory UI")]
    [SerializeField] Transform inventorySlotParent;
    [SerializeField] Transform stashSlotParent;
    [SerializeField] Transform equipmentParent;
    private ItemSlot_UI[] invetoryItemSlot_UI;
    private ItemSlot_UI[] stashItemSlot_UI;
    private EquipmentSlot_UI[] equipmentSlot_UI;

    void Start()
    {
        invetoryItemSlot_UI = inventorySlotParent.GetComponentsInChildren<ItemSlot_UI>();
        stashItemSlot_UI = stashSlotParent.GetComponentsInChildren<ItemSlot_UI>();
        equipmentSlot_UI = equipmentParent.GetComponentsInChildren<EquipmentSlot_UI>();

        for (int i = 0; i < staringItem.Count; i++)
        {
            AddItem(staringItem[i]);
        }
    }

    public void EquipItem(ItemData itemData)
    {
        ItemData_Equip newEquipment = itemData as ItemData_Equip;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equip oldEquipment = null;
        foreach(KeyValuePair<ItemData_Equip, InventoryItem> item in equipmentDictionary)
        {
            if(item.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = item.Key;
            }
        }

        if(oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();

        RemoveItem(itemData);
        UpdateSlotUI();
    }

    public void UnequipItem(ItemData_Equip itemToRemove)
    {
        if(equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }
    }

    public void UpdateSlotUI()
    {
        for(int i =0; i< equipmentSlot_UI.Length; i++)
        {
            foreach(KeyValuePair<ItemData_Equip, InventoryItem> item in equipmentDictionary)
            {
                if(item.Key.equipmentType == equipmentSlot_UI[i].equipmentType)
                {
                    equipmentSlot_UI[i].UpdateItemSlot(item.Value);
                }
            }
            
        }

        for(int i =0; i< invetoryItemSlot_UI.Length; i++)
        {
            invetoryItemSlot_UI[i].ClearUpSlot();
        }

        for(int i =0; i< stashItemSlot_UI.Length; i++)
        {
            stashItemSlot_UI[i].ClearUpSlot();
        }

        for(int i =0; i< inventory.Count; i++)
        {
            invetoryItemSlot_UI[i].UpdateItemSlot(inventory[i]);
            // load từng cái itemSlot với từng cái trong inventoryItems
        }

        for(int i =0; i< stash.Count; i++)
        {
            stashItemSlot_UI[i].UpdateItemSlot(stash[i]);
        }
    }

    public void AddItem(ItemData itemData)
    {
        if (itemData.itemType == ItemType.Equipment)
        {
            AddToInventory(itemData);
        }
        else
        {
            AddToStash(itemData);
        }
        

        UpdateSlotUI();
    }

    private void AddToStash(ItemData itemData)
    {
        if (stashDictionary.ContainsKey(itemData))
        {
            stashDictionary[itemData].AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            stash.Add(newItem);
            stashDictionary.Add(itemData, newItem);
        }
    }


    private void AddToInventory(ItemData itemData)
    {
        if (inventoryDictionary.ContainsKey(itemData))
        {
            inventoryDictionary[itemData].AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            inventoryDictionary.Add(itemData, newItem);
        }
    }

    public void RemoveItem(ItemData itemData)
    {
        if (inventoryDictionary.TryGetValue(itemData, out InventoryItem existingItem))
        {
            if(existingItem.stackSize <= 1)
            {
                inventory.Remove(existingItem);
                inventoryDictionary.Remove(itemData);
            }
            else
            {
                existingItem.RemoveStack();
            }
        }

        if (stashDictionary.TryGetValue(itemData, out InventoryItem stashValue))
        {
            if(stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(itemData);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }

        UpdateSlotUI();
    }

    public List<InventoryItem> GetEquipmentList() => equipment;
    public ItemData_Equip GetEquipment(EquipmentType equipmentType)
    {
        ItemData_Equip equipedItem = null;
        foreach(KeyValuePair<ItemData_Equip, InventoryItem> item in equipmentDictionary)
        {
            if(item.Key.equipmentType == equipmentType)
            {
                equipedItem = item.Key;
            }
        }

        return equipedItem;
    }

}
