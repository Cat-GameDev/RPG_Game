using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    [SerializeField] Enemy enemy;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Character playerCharacter = Cache.GetCharacter(other);
        if(playerCharacter)
        {
            enemy.SetTarget(playerCharacter);
            enemy.IncreaseMoveSpeed();
        }
        
    } 

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            return;
        
        enemy.SetTarget(null);
        enemy.ResetMoveSpeed();
    }

}
