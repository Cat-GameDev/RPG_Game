using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : GameUnit
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected float moveSpeed;
    protected float defaultMoveSpeed;
    [SerializeField] protected AttackArea attackArea;
    protected StateMachine stateMachine = new StateMachine();
    protected float stateTimer;
    string currentAnim;

    protected bool isRight;
    public bool IsRight { get => isRight;}

    public CharacterStats characterStats;


    public virtual void OnInit()
    {
        stateMachine.ChangeState(IdleState);
        DeActiveAttack();
        isRight = true;
        characterStats.OnInit();
        defaultMoveSpeed = moveSpeed;
    }


    public virtual void SlowCharacterBy(float slowPercentage, float slowDuration)
    {
        moveSpeed = moveSpeed * (1 - slowPercentage);
        Invoke(nameof(ReturnDefaultSpeed), slowDuration);
    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
        moveSpeed = defaultMoveSpeed;
    }


    public virtual void OnDeath()
    {
        stateMachine.ChangeState(null);
        ChangeAnim(Constants.ANIM_DIE);
    }

    // public virtual void OnDespawn()
    // {
    //     Destroy(gameObject);
    // }

    protected void ActionAttack()
    {
        attackArea.gameObject.SetActive(true);
        
    }

    protected void DeActiveAttack()
    {
        attackArea.gameObject.SetActive(false);
       
    }

    public virtual void AttackOver()
    {
        DeActiveAttack();
    }

    public virtual void AttackStart()
    {
        ActionAttack();
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

    public Vector2 GetDirection(bool isRight)
    {
        return isRight ? Vector2.right : Vector2.left;
    }

    public virtual void IdleState(ref Action onEnter, ref Action onExecute, ref Action onExit) {}



}
