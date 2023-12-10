using UnityEngine;
using System;
using Random = UnityEngine.Random;
using DG.Tweening;

public abstract class Enemy : Character
{
    [SerializeField] float attackRange;
    private Character target;
    [SerializeField] float maxSpeed;
    float defaultSpeed;
    [SerializeField] float minDistanceToAttack;

    public override void OnInit()
    {
        base.OnInit();
        defaultSpeed = moveSpeed;
    }

    public void Moving()
    {
        ChangeAnim(Constants.ANIM_RUN);
        rb.velocity = transform.right * moveSpeed;
    }

    public void StopMoving()
    {
        ChangeAnim(Constants.ANIM_IDLE);
        rb.velocity = Vector2.zero;
    }

    public void Attack()
    {
        ChangeAnim(Constants.ANIM_ATTACK);
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

        TF.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector2.up * 180f);
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

    internal void IncreaseMoveSpeed()
    {
        moveSpeed = maxSpeed;
    }

    internal void ResetMoveSpeed()
    {
        moveSpeed = defaultSpeed;
    }

    protected override void IdleState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        float timer = 0;
        float randomTime = 0;
        onEnter = () =>
        {
            timer = 0;
            randomTime = Random.Range(1,2);
            StopMoving();
        };

        onExecute = () =>
        {
            timer += Time.deltaTime;
            if(timer > randomTime)
                stateMachine.ChangeState(PatrolState);
        };
    }

    protected void PatrolState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        float timer = 0;
        float randomTime = 0;
        onEnter = () =>
        {
            timer = 0;
            randomTime = Random.Range(1,3);
            
        };

        onExecute = () =>
        {
            timer += Time.deltaTime;
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
                if(timer < randomTime)
                    Moving();
                else
                    stateMachine.ChangeState(IdleState);
            }

        };
    }

    protected void AttackState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        float timer = 0;
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
            timer+=Time.deltaTime;
            if(timer >= 1.5f)
            {
                stateMachine.ChangeState(PatrolState);
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