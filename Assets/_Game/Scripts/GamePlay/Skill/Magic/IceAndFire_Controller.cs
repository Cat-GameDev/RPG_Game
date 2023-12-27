using UnityEngine;

public class IceAndFire_Controller : GameUnit
{
    [SerializeField] Rigidbody2D rb;
    public const float TIME_SELF_DESPAWN = 4f;
    public void OnInit(float speed)
    {
        rb.velocity = transform.right * speed;
        Invoke(nameof(OnDespawn), TIME_SELF_DESPAWN);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = Cache.GetEnemy(other);
        if(enemy)
        {
            PlayerStats playerStats = LevelManager.Instance.Player.characterStats as PlayerStats;
            EnemyStats enemyTarget = enemy.characterStats as EnemyStats;
            playerStats.DoMagicalDamage(enemyTarget);
        }
    }
}