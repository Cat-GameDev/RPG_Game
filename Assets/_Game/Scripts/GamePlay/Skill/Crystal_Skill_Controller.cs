using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Crystal_Skill_Controller : GameUnit
{
    public const float TIME_EXPLODE = 0.4f;
    [SerializeField] Animator anim;
    [SerializeField] CircleCollider2D circle;
    string currentAnim;
    bool canMove;
    public List<Enemy> enemies = new List<Enemy>();
    float moveSpeed;
    Enemy currentTarget;
    Player player;

    void Update()
    {
        if(canMove)
        {
            if (SkillManager.Instance.Clone_Skill.CrystalInsteadOfClone)
            {
                if (enemies.Count > 0)
                {
                    if (!currentTarget)
                    {
                        currentTarget = enemies[Random.Range(0, enemies.Count)];
                    }
                    TF.position = Vector2.MoveTowards(TF.position, currentTarget.transform.position, moveSpeed * Time.deltaTime);
                    if (Vector2.Distance(TF.position, currentTarget.transform.position) < 0.5f)
                    {
                        Explode();
                        currentTarget = null;
                    }
                }
            }
            else
            {
                // di chuyển Enemy target gần nhất trong enemies
                TF.position = Vector2.MoveTowards(TF.position, ClosestTarget(), moveSpeed * Time.deltaTime);
                if(Vector2.Distance(TF.position, ClosestTarget()) < 0.5f)
                {
                    Explode();
                }
            }

        }
    }

    public void OnInit(float moveSpeed, bool canMove, Player player)
    {
        this.player = player;
        this.moveSpeed = moveSpeed;
        this.canMove = canMove;
        TF.localScale = Vector2.one;
        ChangeAnim(Constants.ANIM_IDLE);
        circle.enabled = false;
        enemies.Clear();
        CheckEnemyCloest();
    }


    private void CheckEnemyCloest()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(TF.position, 10f);
        foreach (Collider2D hit in colliders)
        {
            Enemy hitEnemy = Cache.GetEnemy(hit);
            if (hitEnemy)
            {
                enemies.Add(hitEnemy);
            }

        }
    }

    public Vector3 ClosestTarget()
    {
        if (enemies.Count > 0)
        {
            Enemy closestEnemy = null;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < enemies.Count; i++)
            {
                float distance = Vector2.Distance(TF.position, enemies[i].transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemies[i];
                }
            }

            if (closestEnemy != null)
            {
                return closestEnemy.transform.position;
            }
        }

        // If there are no enemies or an issue occurred, return the Crystal_Skill_Controller's current position
        return TF.position;
    }

    public void Explode()
    {
        circle.enabled = true;
        ChangeAnim(Constants.ANIM_EXPLODE);
        Invoke(nameof(OnDespawn), TIME_EXPLODE);
    }

    private void ChangeAnim(string currentAnim)
    {
        if(this.currentAnim != currentAnim)
        {
            anim.ResetTrigger(this.currentAnim);
            this.currentAnim = currentAnim;
            anim.SetTrigger(this.currentAnim);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        CharacterStats enemyStats = Cache.GetCharacterStats(other);

        if(enemyStats)
        {
            player.characterStats.DoDamge(enemyStats);
        }
    }
}
