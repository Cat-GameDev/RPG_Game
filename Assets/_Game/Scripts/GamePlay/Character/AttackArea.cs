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
        }
    }
}