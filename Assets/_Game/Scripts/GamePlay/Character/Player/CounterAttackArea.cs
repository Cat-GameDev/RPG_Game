using UnityEngine;

public class CounterAttackArea : MonoBehaviour
{
    [SerializeField] private Player player; // Make sure to mark this private
    private Enemy currentEnemy; // Keep track of the current enemy

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = Cache.GetEnemy(other);
        if (enemy != null)
        {
            currentEnemy = enemy; // Store the current enemy
            InvokeRepeating(nameof(SetIsSuccessfulCounterAttackRepeatedly), 0, 0.1f);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Enemy enemy = Cache.GetEnemy(other);
        if (enemy != null && enemy == currentEnemy)
        {
            // If the exiting enemy is the current enemy, stop the repeating invocation
            CancelInvoke(nameof(SetIsSuccessfulCounterAttackRepeatedly));
            currentEnemy = null; // Reset current enemy
        }
    }

    void SetIsSuccessfulCounterAttackRepeatedly()
    {
        if (currentEnemy != null)
        {
            player.SetIsSuccessfulCounterAttack(currentEnemy.CanBeStunned(), currentEnemy);
        }
    }
}