using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Controller : GameUnit
{
    public const float TIME_SELF_DESPAWN = .5f;
    public void OnInit()
    {
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
