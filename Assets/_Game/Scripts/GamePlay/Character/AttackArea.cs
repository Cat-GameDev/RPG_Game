using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField] Character character;
    void OnTriggerEnter2D(Collider2D other)
    {
        IHit hit = Cache.GetIHit(other);
        if (hit != null)
        {
            hit.OnHit(character.Damage); 
        }
    }
}