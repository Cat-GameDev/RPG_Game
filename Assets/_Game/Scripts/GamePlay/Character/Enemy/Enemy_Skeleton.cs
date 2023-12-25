using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy_Skeleton : Enemy
{
    public const float TIME_ONDESPAWN = 1.5f;
    void Update()
    {
        if(isKnockback || characterStats.IsDead)
            return;

        stateMachine?.Execute();
    }

    public override void OnInit()
    {
        base.OnInit();
        offset = new Vector3(1.7f,.23f,0f);
    }

    public override Vector3 GetPositionOnHead()
    {
        return TF.position + new Vector3(0f, 1.5f, 0f);
    }

    public override Vector3 GetOffset(bool isRight)
    {
        if(isRight)
            return TF.position + new Vector3(-offset.x, offset.y,0);
        
        return TF.position  + offset;
    }

    public override void StopMoving()
    {
        ChangeAnim(Random.Range(0,2) == 0 ? Constants.ANIM_IDLE : Constants.ANIM_REACT);
        rb.velocity = Vector2.zero;
    }

    public override void OnDeath()
    {
        base.OnDeath();
        Invoke(nameof(OnDespawn), TIME_ONDESPAWN);
    }









}