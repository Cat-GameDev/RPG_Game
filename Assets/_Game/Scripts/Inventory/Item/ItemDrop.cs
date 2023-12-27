
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] int possibleItemDrop;
    [SerializeField] ItemData[] possibleDrop;
    List<ItemData> dropList = new List<ItemData>();

    public virtual void GenerateDrop()
    {
        if(possibleDrop.Length <= 0) return;

        for(int i =0; i< possibleDrop.Length; i++)
        {
            if(Random.Range(1, 100) <= possibleDrop[i].dropChance)
            {
                dropList.Add(possibleDrop[i]);
            }
        }

        for(int i=0; i < possibleItemDrop; i++)
        {
            if(dropList.Count > 0)
            {
                ItemData itemRandom = dropList[Random.Range(0, dropList.Count -1)];
                dropList.Remove(itemRandom);
                Dropitem(itemRandom);
            }
        }

        dropList.Clear();
    }


    protected void Dropitem(ItemData itemData)
    {
        ItemObject itemDrop = SimplePool.Spawn<ItemObject>(PoolType.Item_Object, transform.position, Quaternion.identity);

        Vector2 velocityRandom = new Vector2(Random.Range(-5,5), Random.Range(12,15));
        itemDrop.SetupItem(itemData, velocityRandom);  
    }
}
