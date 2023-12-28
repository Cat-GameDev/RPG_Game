using System;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    public static readonly Vector3 HEALTHBAR_POSITION = new Vector3(0f, 1.7f, 0f);
    [SerializeField] Player player;
    public override void OnDeath()
    {
        base.OnDeath();
        healthBar_UI.OnDespawn();
        player.OnDeath();
    }

    protected override void CreateHealthBar()
    {
        if(healthBar_UI == null)
        {
            healthBar_UI = SimplePool.Spawn<HealthBar_UI>(PoolType.HealthBar_UI);
            healthBar_UI.OnInit(maxHealth.GetValue(), player.TF, HEALTHBAR_POSITION);
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        CreateHealthBar();
    }

    internal float GetMaxHealthValue()
    {
        return maxHealth.GetValue();
    }

}