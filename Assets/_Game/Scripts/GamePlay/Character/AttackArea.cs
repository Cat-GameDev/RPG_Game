using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField] CharacterStats characterStats;
    void OnTriggerEnter2D(Collider2D other)
    {
        Character character = Cache.GetCharacter(other);
        if (character != null)
        {
            characterStats.DoDamge(character.characterStats);
        }
    }
}