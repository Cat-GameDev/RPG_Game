using UnityEngine;

public class EnemyStats : CharacterStats
{
    [SerializeField] Enemy enemy;

    public Enemy Enemy { get => enemy; }

    public override void OnHit(float damage)
    {
        if(!IsDead)
        {
            enemy.StartCoroutine(enemy.HitKnockback());
        }
        
        base.OnHit(damage);
    }

    public override void OnDeath()
    {
        enemy.OnDeath();
    }

}