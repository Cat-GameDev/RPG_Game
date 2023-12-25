using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField] Character ch;
    void OnTriggerEnter2D(Collider2D other)
    {
        Character character = Cache.GetCharacter(other);
        if (character != null)
        {
            ch.characterStats.DoDamge(character.characterStats);

            
            // Only enemy 
            if(character is Player)
                return;

            //inventory get weapon call in effect 
            Inventory.Instance.GetEquipment(EquipmentType.Weapon)?.ExecuteItemEffect(character.TF);
            
            
        }
    }
}