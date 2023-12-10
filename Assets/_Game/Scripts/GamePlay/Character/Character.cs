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
    string currentAnim;
    public float hp;
    public bool IsDead => hp <= 0;

    public float Damage { get => damage; }

    protected bool isRight;
    [SerializeField] CharacterFX characterFX;

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDir;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnockback;

    public virtual void OnInit()
    {
        hp = 1000;
        stateMachine.ChangeState(IdleState);
        DeActiveAttack();
        isRight = true;
        isKnockback = false;
    }

    public void OnHit(float damage)
    {
        if(!IsDead)
        {
            hp -= damage;
            characterFX.StartCoroutine(nameof(characterFX.FlashFX));
            StartCoroutine(HitKnockback());
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

    protected void ChangeAnim(string animName)
    {
        if(currentAnim != animName)
        {
            anim.ResetTrigger(currentAnim);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
        }
    }

    protected Vector2 GetDirection(bool isRight)
    {
        return isRight ? Vector2.right : Vector2.left;
    }

    protected IEnumerator HitKnockback()
    {
        isKnockback = true;
        rb.velocity = new Vector2(knockbackDir.x * -GetDirection(isRight).x, knockbackDir.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnockback = false;
    }

    protected virtual void IdleState(ref Action onEnter, ref Action onExecute, ref Action onExit) {}



}
