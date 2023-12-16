using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy_Skeleton : Enemy
{
    [SerializeField] CapsuleCollider2D capsuleCollider2D;
    void Update()
    {
        if(isKnockback)
            return;

        stateMachine?.Execute();
        Debug.Log(stateMachine.name);
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

    public override Vector3 GetOffset()
    {
        return offset;
    }

    public override Vector3 GetSize()
    {
        return capsuleCollider2D.size;
    }

    public override void StopMoving()
    {
        ChangeAnim(Random.Range(0,2) == 0 ? Constants.ANIM_IDLE : Constants.ANIM_REACT);
        rb.velocity = Vector2.zero;
    }









}