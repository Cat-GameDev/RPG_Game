using UnityEngine;

public class EnemyStats : CharacterStats
{
    public static readonly Vector3 HEALTHBAR_POSITION = new Vector3(0f, 1.5f, 0f);

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
        healthBar_UI.OnDespawn();
        healthBar_UI = null;
        enemy.OnDeath();
    }

    protected override void CreateHealthBar()
    {
        if(healthBar_UI == null)
        {
            healthBar_UI = SimplePool.Spawn<HealthBar_UI>(PoolType.HealthBar_UI);
            healthBar_UI.OnInit(hp.GetValue(), enemy.TF, HEALTHBAR_POSITION);
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        CreateHealthBar();
    }

}