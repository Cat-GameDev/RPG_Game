using System.Text;
using UnityEngine;

public enum ItemType
{
    Material,
    Equipment,
}

[CreateAssetMenu(fileName ="ItemData", menuName = "Data/Item", order = 1)]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    protected StringBuilder sb = new StringBuilder();


    [Range(0, 100)]
    public float dropChance;

    public virtual string GetDescription()
    {
        return "";
    }
    
}
