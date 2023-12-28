
using UnityEngine;
using UnityEngine.EventSystems;



public class UI_CraftSlot : ItemSlot_UI
{
    UI_CraftWindow craftWindow;
    void Start()
    {
        craftWindow = FindObjectOfType<UI_CraftWindow>();
    }
    public void SetupCraftSlot(ItemData_Equip _data)
    {
        if (_data == null)
            return;

        item.itemData = _data;

        image.sprite = _data.icon;
        text.text = _data.itemName;

        if (text.text.Length > 12)
            text.fontSize = text.fontSize * .7f;
        else
            text.fontSize = 24;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        craftWindow.SetupCraftWindow(item.itemData as ItemData_Equip);
    }
}
