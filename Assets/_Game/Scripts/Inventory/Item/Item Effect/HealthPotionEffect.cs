using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionEffect : GameUnit
{
    int level;
    [SerializeField] float recoverHp;
    [SerializeField] private float percantageModifier = .4f;
    public void OnInit()
    {
        level = LevelManager.Instance.currentLevel;
        IncreseRecoverHpByLvel(level);
    }

    private void IncreseRecoverHpByLvel(int level)
    {
        for (int i = 1; i < level; i++)
        {
            recoverHp *= percantageModifier;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(Constants.PLAYER_TAG))
        {
            Player player = Cache.GetPlayer(other);
            if(player.characterStats.IsDead)
                return;

            player.characterStats.RecoverHealth(recoverHp);
        }
        
    }
}
