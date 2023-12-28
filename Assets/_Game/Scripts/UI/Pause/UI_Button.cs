using UnityEngine;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour
{
    [SerializeField] Button equipButton;
    [SerializeField] Button deleteButton;

    public void AddListener(ItemSlot_UI itemSlot)
    {
        equipButton.onClick.AddListener(itemSlot.EquipItem);
        deleteButton.onClick.AddListener(itemSlot.DeleteItem);
    }

    public void RemoveAđListener(ItemSlot_UI itemSlot)
    {
        equipButton.onClick.RemoveListener(itemSlot.EquipItem);
        deleteButton.onClick.RemoveListener(itemSlot.DeleteItem);
    }
}