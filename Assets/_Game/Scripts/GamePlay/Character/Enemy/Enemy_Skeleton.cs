using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy_Skeleton : Enemy
{
    public override void StopMoving()
    {
        ChangeAnim(Random.Range(0,2) == 0 ? Constants.ANIM_IDLE : Constants.ANIM_REACT);
        rb.velocity = Vector2.zero;
    }


    void Update()
    {
        if(isKnockback)
            return;

        stateMachine?.Execute();
    }




}