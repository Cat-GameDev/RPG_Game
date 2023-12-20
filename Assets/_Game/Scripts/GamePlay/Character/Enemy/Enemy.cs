using UnityEngine;
using System;
using Random = UnityEngine.Random;
using DG.Tweening;
using System.Collections;

public abstract class Enemy : Character
{
    public const float ResetAttackTimer = 1.5f;
    [SerializeField] protected float attackRange;
    protected Character target;
    [SerializeField] protected float maxSpeed;
    float defaultSpeed;
    [SerializeField] protected float minDistanceToAttack;

    [Header("Stun info")]
    [SerializeField] protected float stunDuration;
    [SerializeField] protected Vector2 stunDirection;
    protected bool canBeStuned;

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDir;
    [SerializeField] protected float knockbackDuration;
    protected Vector3 offset;
    protected bool isKnockback;
    protected bool isFreeze;

    public override void OnInit()
    {
        base.OnInit();
        defaultSpeed = moveSpeed;
        stateTimer = 0;
        canBeStuned = isKnockback = false;
    }

    public abstract Vector3 GetPositionOnHead();
    public abstract Vector3 GetOffset(bool isRight);

    public void Moving()
    {
        ChangeAnim(Constants.ANIM_RUN);
        rb.velocity = transform.right * moveSpeed;
    }

    public virtual void StopMoving()
    {
        ChangeAnim(Constants.ANIM_IDLE);
        rb.velocity = Vector2.zero;
    }

    public void Attack()
    {
        ChangeAnim(Constants.ANIM_ATTACK);
    }

    public void FreezeState()
    {
        stateMachine.ChangeState(FreezeState);
    }

    public IEnumerator HitKnockback()
    {
        isKnockback = true;
        rb.velocity = new Vector2(knockbackDir.x * -GetDirection(isRight).x, knockbackDir.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnockback = false;
    }


    // private bool IsTargetInRange()
    // {   
    //     if(target != null && Vector2.Distance(target.TF.position, TF.position) <= attackRange)
    //         return  true;
    //     return false;
    // }

    private bool IsTargetInRange()
    {
        if (target != null)
        {
            float distanceToTarget = Vector2.Distance(target.transform.position, transform.position);
            if (distanceToTarget <= minDistanceToAttack)
            {
                MoveAwayFromTarget();
                return false;
            }

            // Kiểm tra tầm đánh
            return distanceToTarget <= attackRange;
        }
        return false;
    }

    private void MoveAwayFromTarget()
    {
        ChangeAnim(Constants.ANIM_RUN);
        Vector2 awayDirection = (TF.position - target.TF.position).normalized;
        float newXPosition = TF.position.x + awayDirection.x * minDistanceToAttack;

        TF.DOMoveX(newXPosition, 1f).SetEase(Ease.OutQuart);
    }
    
    public void ChangeDirection(bool isRight)
    {
        this.isRight = isRight;

        TF.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180f);
    }

    internal void SetTarget(Character character)
    {
        this.target = character;

        if(IsTargetInRange())
        {
            stateMachine.ChangeState(AttackState);
        }
        else if(target != null)
        {
            stateMachine.ChangeState(PatrolState);
        } 
        else stateMachine.ChangeState(IdleState);
    }

    public void OpenCounterAttackWindow() => canBeStuned = true;
    public void CloseCounterAttackWindow() => canBeStuned = false;


    public virtual bool CanBeStunned()
    {
        if(canBeStuned)
        {
            canBeStuned = false;
            stateMachine.ChangeState(StunState);
            return true;
        }
        return false;
    }

    internal void IncreaseMoveSpeed()
    {
        moveSpeed = maxSpeed;
    }

    internal void ResetMoveSpeed()
    {
        moveSpeed = defaultSpeed;
    }

    public override void IdleState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        float randomTime = 0;
        onEnter = () =>
        {
            stateTimer = 0;
            randomTime = Random.Range(1,2);
            StopMoving();
        };

        onExecute = () =>
        {
            stateTimer += Time.deltaTime;
            if(isFreeze)
                return;

            if(stateTimer > randomTime)
                stateMachine.ChangeState(PatrolState);
        };
    }

    protected void PatrolState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        float randomTime = 0;
        onEnter = () =>
        {
            stateTimer = 0;
            randomTime = Random.Range(1,3);
            
        };

        onExecute = () =>
        {
            stateTimer += Time.deltaTime;
            if(target != null)
            {
                ChangeDirection(target.transform.position.x > TF.position.x);
                if(IsTargetInRange())
                {
                    stateMachine.ChangeState(AttackState);
                }
                else Moving();
                
            }
            else 
            {
                if(stateTimer < randomTime)
                    Moving();
                else
                    stateMachine.ChangeState(IdleState);
            }

        };
    }

    protected void AttackState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        onEnter = () =>
        {
            if(target!= null)
            {
                ChangeDirection(target.transform.position.x > TF.position.x);
                StopMoving();
                Attack();
            }
        };

        onExecute = () =>
        {
            stateTimer+=Time.deltaTime;
            if(stateTimer >= ResetAttackTimer)
            {
                stateMachine.ChangeState(PatrolState);
            }
        };
    }

    protected virtual void StunState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {

        onEnter = () =>
        {
            ChangeAnim(Constants.ANIM_STUN);
            stateTimer = 0;
            rb.AddForce(stunDirection * -GetDirection(isRight), ForceMode2D.Impulse);
            characterStats.CharacterFX.InvokeRepeating(nameof(characterStats.CharacterFX.RedColorBlink), 0, 0.1f);
        };

        onExecute = () =>
        {
            stateTimer += Time.deltaTime;
            if(stateTimer > stunDuration)
            {
                stateMachine.ChangeState(IdleState);
            }
        };

        onExit = () =>
        {
            characterStats.CharacterFX.CanelChangeColor();
        };
    }

    protected virtual void FreezeState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        onEnter = () =>
        {
            rb.velocity = Vector2.zero;
            isFreeze = true;
            ChangeAnim(Constants.ANIM_FREEZE);
        };

        onExecute = () =>
        {
            if(GameManager.Instance.IsState(GameState.Gameplay))
            {
                stateMachine.ChangeState(IdleState);
                isFreeze = false;
            }
        };

    }


    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(Constants.ENEMY_WALL))
        {
            ChangeDirection(!isRight);
        }
    }


}