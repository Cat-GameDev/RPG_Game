using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlot_UI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image image;
    public InventoryItem item;

    public void UpdateItemSlot(InventoryItem newItem)
    {
        item = newItem;
        image.color = Color.white;
        image.raycastTarget = true;

        if (item != null)
        {
            image.sprite = item.itemData.icon;

            if (item.stackSize > 1)
            {
                text.SetText(item.stackSize.ToString());
            }
            else
            {
                text.SetText("");
            }
        }
    }

    public void ClearUpSlot()
    {
        item = null;

        image.sprite = null;
        image.color = Color.clear;
        image.raycastTarget = false;
        text.SetText("");
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if(item == null)
            return;

        if(item.itemData.itemType == ItemType.Equipment)
        {
            Inventory.Instance.EquipItem(item.itemData);
        }
    }
}