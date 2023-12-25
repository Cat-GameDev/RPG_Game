using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Blackhole_Skill_Controller : GameUnit
{
    public const float ATTACK_DELAY = 0.2f;
    public const float ACTIVE_ATTACK = 1.5f;
    float maxSize;
    float growSpeed;
    public List<Enemy> targetEnemy = new List<Enemy>();

    bool canAttack;
    int attackAmount;
    bool isAttacking;

    public bool CanAttack { get => canAttack;}

    void Update()
    {
        
        if (canAttack && attackAmount > 0 && !isAttacking)
        {
            if(targetEnemy.Count > 0)
                StartCoroutine(AttackWithDelay());
        }


        TF.localScale = Vector2.Lerp(TF.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        
    }


    public void OnInit()
    {
        targetEnemy.Clear();
        canAttack = isAttacking = false;
        TF.localScale = Vector3.one;
        Invoke(nameof(ActiveAttack), ACTIVE_ATTACK);
        SelfDespawn();
    }

    private void CreateCloneLeft(Enemy enemy)
    {
        if(enemy != null)
        {
            SkillManager.Instance.Clone_Skill.CreateClone(enemy.GetOffset(false), false, true, 10f);
        }
        
    }

    private void CreateCloneRight(Enemy enemy)
    {
        if(enemy != null)
        {
            SkillManager.Instance.Clone_Skill.CreateClone(enemy.GetOffset(true), true, true, 10f);
        }
    }
        
    

    private IEnumerator AttackWithDelay()
    {
        isAttacking = true;

        int random = Random.Range(0, targetEnemy.Count);
        CreateCloneLeft(targetEnemy[random]);
        
        

        yield return new WaitForSeconds(ATTACK_DELAY);
        
        CreateCloneRight(targetEnemy[random]);

        attackAmount--;

        yield return new WaitForSeconds(ATTACK_DELAY);
        isAttacking = false;
    }



    private void ActiveAttack() => canAttack = true;
    
    private void SelfDespawn()
    {
        Invoke(nameof(OnDespawn), Constants.TIME_ULTIMATE_SKILL);
    }

    public void Setup(float maxSize, float growSpeed,int attackAmount)
    {
        this.maxSize = maxSize;
        this.growSpeed = growSpeed;
        this.attackAmount = attackAmount;
    }

    private void ShowTarget(Enemy enemy)
    {
        UITickText tickTextUI = SimplePool.Spawn<UITickText>(PoolType.UITickText, enemy.TF.position, Quaternion.identity);
        tickTextUI.SetupText(enemy.GetPositionOnHead(), this);
    }
    

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = Cache.GetEnemy(other);
        if(enemy)
        {
            enemy.FreezeState();
            ShowTarget(enemy);
            targetEnemy.Add(enemy);
        }
    }


}