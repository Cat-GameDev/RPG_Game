using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEditor.PackageManager;

public class Sword_Skil_Controller : GameUnit
{
    public const float TIME_RETURN_SWORD = 2f;
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] CircleCollider2D circleCollider2D;
    Player player;
    float returnSpeed;
    string currentAnim;
    bool isReturning;
    public float maxSpeed = 10f;

    //Bounce
    bool isBouncing;
    float bounceAmount, bounceSpeed;
    List<Enemy> enemyTargets = new List<Enemy>();
    int targetIndex;

    //Pierce
    float pierceAmount;

    //Spin
    float maxDistance, spinDuration, spinTimer;
    bool wasStopped;
    bool isSpining;

    float hitTimer, hitCooldown;
    float freezeTime;
    
    float spinDirection;


    void Start()
    {
        player = LevelManager.Instance.Player;
    }

    void Update()
    {
        if (isReturning)
        {
            Return();
        }
        
        if (isBouncing && enemyTargets.Count > 0)
        {
            Bounce();
        }
        
        if(isSpining)
        {
            Spin();
        }

    }

    public void OnInit()
    {
        TF.parent = null;
        
        circleCollider2D.enabled = true;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints2D.None;
        isReturning = isBouncing = isSpining = wasStopped = false;

        spinDirection = Mathf.Clamp(rb.velocity.x, -1 , 1);
        targetIndex = 0;
        enemyTargets.Clear();
        Invoke(nameof(AutoReturn), 6f);
    }

    public void AutoReturn()
    {
        circleCollider2D.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        ReturnSword();
    }

    private void Spin()
    {
        if(Vector2.Distance(player.TF.position, TF.position) > maxDistance && !wasStopped)
        {
            StopWhenSpining();
        }

        if (wasStopped)
        {
            spinTimer -= Time.deltaTime;

            TF.position = Vector2.MoveTowards(TF.position, new Vector2(TF.position.x + spinDirection, 
                                                                                        TF.position.y), 1.5f * Time.deltaTime);

            if(spinTimer < 0)
            {
                isReturning = true;
                isSpining = false;
            }

            hitTimer -= Time.deltaTime;

            if(hitTimer < 0)
            {
                hitTimer = hitCooldown;
                
                Collider2D[] colliders = Physics2D.OverlapCircleAll(TF.position, 1f);

                foreach (Collider2D hit in colliders)
                {
                    CharacterStats enemyStats = Cache.GetCharacterStats(hit);

                    if(enemyStats)
                    {
                        SwordDoDamage(enemyStats);
                    }

                }

            }
        }
    }

    private void SwordDoDamage(CharacterStats enemyStats)
    {
        player.characterStats.DoDamge(enemyStats);
        Inventory.Instance.GetEquipment(EquipmentType.Amulet)?.ExecuteItemEffect(enemyStats.transform);
    }

    private void StopWhenSpining()
    {
        ChangeAnim(Constants.ANIM_FLIP);
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        spinTimer = spinDuration;
    }

    private void Return()
    {
        ChangeAnim(Constants.ANIM_FLIP);
        TF.position = Vector2.Lerp(TF.position, player.TF.position, returnSpeed * Time.deltaTime);

        if (Vector2.Distance(TF.position, player.TF.position) < 2f)
        {
            returnSpeed = returnSpeed + maxSpeed;
        }

        if (Vector2.Distance(TF.position, player.TF.position) < 1.5f)
        {
            ChangeAnim(Constants.ANIM_IDLE);
        }

        if (Vector2.Distance(TF.position, player.TF.position) < 1.1f)
        {
            player.CatchTheSword();
            OnDespawn();
            isReturning = false;
        }
    }

    private void Bounce()
    {
        ChangeAnim(Constants.ANIM_FLIP);
        if(enemyTargets[targetIndex] == null)
            return;

        TF.position = Vector2.MoveTowards(TF.position, enemyTargets[targetIndex].TF.position, bounceSpeed * Time.deltaTime);
        if (Vector2.Distance(TF.position, enemyTargets[targetIndex].TF.position) < 0.1f)
        {
            //enemyTargets[targetIndex].OnHit(player.characterStats.damage.GetValue());
            SwordDoDamage(enemyTargets[targetIndex].characterStats);
            targetIndex++;
            bounceAmount--;

            if (bounceAmount < 0)
            {
                isBouncing = false;
                isReturning = true;
            }

            if (targetIndex >= enemyTargets.Count)
            {
                targetIndex = 0;
            }
        }
        
    }


    public void SetupSpin(bool isSpining,float maxDistance,float spinDuration, float hitCooldown)
    {
        this.isSpining = isSpining;
        this.maxDistance = maxDistance;
        this.spinDuration = spinDuration;
        this.hitCooldown = hitCooldown;
    }

    public void SetupPierce(float pierceAmount)
    {
        this.pierceAmount = pierceAmount;
    }

    public void SetUpBounce(bool isBouncing, float bounceSpeed, float bounceAmount)
    {
        this.isBouncing = isBouncing;
        this.bounceSpeed = bounceSpeed;
        this.bounceAmount = bounceAmount;
    }


    public void SetupSword(float throwSpeed, float returnSpeed, float freezeTime)
    {
        rb.velocity = transform.right * throwSpeed;
        this.returnSpeed = returnSpeed;
        this.freezeTime = freezeTime;
    }

    
    public void ChangeAnim(string animName)
    {
        if(currentAnim != animName)
        {
            anim.ResetTrigger(currentAnim);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
        }
    }

    public void ReturnSword()
    {
        rb.isKinematic = false;
        isReturning = true;
        TF.parent = null;
        circleCollider2D.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        CharacterStats enemyStats = Cache.GetCharacterStats(other);

        if(enemyStats)
        {
            SwordDoDamage(enemyStats);
            Enemy enemy = Cache.GetEnemy(other);
            enemy.FreezeState();
            GetBounceTarget();
        }



        if (isReturning)
        {
            return;
        }
           

        if(isSpining)
        {
            StopWhenSpining();
            return;
        }




        if (pierceAmount > 0 && enemyStats)
        {
            pierceAmount--;
            return;
        }

        SwordStuck(other);
    }

    private void GetBounceTarget()
    {
        if (isBouncing && enemyTargets.Count <= 0)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(TF.position, 10f);

            // var : tự động gắn biến cho nó đúng
            foreach (Collider2D hit in colliders)
            {
                Enemy hitEnemy = Cache.GetEnemy(hit);
                if (hitEnemy)
                {
                    enemyTargets.Add(hitEnemy);
                }

            }
        }
    }

    private void SwordStuck(Collider2D other)
    {

        circleCollider2D.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if(isBouncing && enemyTargets.Count > 0)
            return;
        
        TF.parent = other.transform;
        Invoke(nameof(ReturnSword), TIME_RETURN_SWORD);
    }
}
