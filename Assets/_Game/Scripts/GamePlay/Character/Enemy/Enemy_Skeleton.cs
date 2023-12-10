using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    void Update()
    {
        if(isKnockback)
            return;
    
        stateMachine?.Execute();
    }

    
}