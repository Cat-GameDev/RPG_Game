using UnityEngine;

public class EnemyStats : CharacterStats
{
    public static readonly Vector3 HEALTHBAR_POSITION = new Vector3(0f, 1.5f, 0f);

    [SerializeField] Enemy enemy;

    public Enemy Enemy { get => enemy; }

    [Header("Level details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percantageModifier = .4f;

    public override void OnInit()
    {
        ApplyLevelModifiers();
        base.OnInit();
        CreateHealthBar();
    }

    private void ApplyLevelModifiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(hp);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);

        //Modify(soulsDropAmount);
    }

    private void Modify(Stats _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percantageModifier;

            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

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
        base.OnDeath();
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



}