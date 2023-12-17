using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy_Skeleton : Enemy
{
    void Update()
    {
        if(isKnockback)
            return;

        stateMachine?.Execute();
        //Debug.Log(stateMachine.name);
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









}