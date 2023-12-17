using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class Blackhole_Skill_Controller : GameUnit
{
    float maxSize;
    float growSpeed;
    bool canGrow;
    public List<Enemy> targetEnemy = new List<Enemy>();

    public bool canAttack;
    public int attackAmount = 4;
    public float cloneAttackCooldown = .3f;
    float cloneAttackTimer;

    void Update()
    {
        
        if(!GameManager.Instance.IsState(GameState.UltimateSkill))
        {
            OnDespawn();
        }

    
        cloneAttackTimer -= Time.deltaTime;
        if(canAttack && cloneAttackTimer < 0)
        {
            cloneAttackTimer = cloneAttackCooldown;
            int random = Random.Range(0 , targetEnemy.Count);
            SkillManager.Instance.Clone_Skill.CreateClone(targetEnemy[random].TF.position, true, true, 10f);
        }

        if(canGrow)
        {
            TF.localScale = Vector2.Lerp(TF.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
    }

    public void OnInit()
    {
        canGrow = true;
        TF.localScale = Vector3.one;
    }

    public void Setup(float maxSize, float growSpeed)
    {
        this.maxSize = maxSize;
        this.growSpeed = growSpeed;
    }

    

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = Cache.GetEnemy(other);
        if(enemy)
        {
            enemy.FreezeState();
            UITickText tickTextUI =  SimplePool.Spawn<UITickText>(PoolType.UITickText, enemy.TF.position, Quaternion.identity);
            tickTextUI.OnInit();
            tickTextUI.SetupText(enemy.GetPositionOnHead(), enemy.GetSize(), enemy.GetOffset());
            targetEnemy.Add(enemy);
        }
    }
}