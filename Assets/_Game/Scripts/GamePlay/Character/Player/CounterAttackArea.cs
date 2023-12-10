using UnityEngine;

public class CounterAttackArea : MonoBehaviour
{
    [SerializeField] Player player;
    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = Cache.GetEnemy(other);
        player.SetIsSuccessfulCounterAttack(enemy.CanBeStuned());
    }
}