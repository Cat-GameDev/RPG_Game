using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class Blackhole_Skill_Controller : GameUnit
{
    public float maxSize;
    public float growSpeed;
    public bool canGrow;
    public List<Enemy> targetEnemy = new List<Enemy>();

    public bool canAttack;
    public int attackAmount = 4;
    public float cloneAttackCooldown = .3f;
    float cloneAttackTimer;

    void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.I))
            canAttack = true;

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

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = Cache.GetEnemy(other);
        if(enemy)
        {
            enemy.FreezeTime(true);
            UITickText tickTextUI =  SimplePool.Spawn<UITickText>(PoolType.UITickText, enemy.TF.position, Quaternion.identity);
            tickTextUI.SetupText(enemy.GetPositionOnHead(), enemy.GetSize());
            targetEnemy.Add(enemy);
        }
    }
}