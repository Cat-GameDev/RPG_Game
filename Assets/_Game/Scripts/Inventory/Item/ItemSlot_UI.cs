using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using JetBrains.Annotations;

public class ItemSlot_UI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected TextMeshProUGUI text;
    [SerializeField] protected Image image;
    public InventoryItem item;
    [SerializeField] UI_Button button;
    [SerializeField] RectTransform itemClicked;
    [SerializeField] UI_ItemTooltip itemTooltip;

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

    public void EquipItem()
    {
        Debug.Log(gameObject.name);
        if(item.itemData.itemType == ItemType.Equipment)
        {
            Inventory.Instance.EquipItem(item.itemData);
        }

        button?.RemoveAđListener(this);
        button?.gameObject.SetActive(false);

        itemClicked?.gameObject.SetActive(false);
    }

    public void DeleteItem()
    {
        if(item.itemData != null)
            Inventory.Instance.RemoveItem(item.itemData);
        
        button?.RemoveAđListener(this);
        button?.gameObject.SetActive(false);

        itemClicked?.gameObject.SetActive(false);
    }

    
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if(item == null)
            return;

        button?.AddListener(this);
        button?.gameObject.SetActive(true);


        if (itemClicked != null)
        {
            itemClicked.position = transform.position;
            itemClicked.gameObject.SetActive(true);
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        itemTooltip?.ShowToolTip(item?.itemData as ItemData_Equip);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemTooltip?.HideToolTip();
    }
}