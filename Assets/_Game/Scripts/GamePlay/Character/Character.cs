using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : GameUnit, IHit
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float damage;
    [SerializeField] protected AttackArea attackArea;
    protected StateMachine stateMachine = new StateMachine();
    protected float stateTimer;
    string currentAnim;
    public float hp;
    public bool IsDead => hp <= 0;

    public float Damage { get => damage; }

    protected bool isRight;
    [SerializeField] CharacterFX characterFX;


    public virtual void OnInit()
    {
        hp = 1000;
        stateMachine.ChangeState(IdleState);
        DeActiveAttack();
        isRight = true;
        
    }

    public virtual void OnHit(float damage)
    {
        if(!IsDead)
        {
            hp -= damage;
            characterFX.StartCoroutine(nameof(characterFX.FlashFX));
            if(IsDead)
            {
                OnDeath();
            }
        }
    }

    protected virtual void OnDeath()
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
