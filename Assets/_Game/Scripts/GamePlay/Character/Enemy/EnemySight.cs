using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    [SerializeField] Enemy enemy;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Character playerCharacter = Cache.GetCharacter(other);
        enemy.SetTarget(playerCharacter);
        enemy.IncreaseMoveSpeed();
    } 

    private void OnTriggerExit2D(Collider2D other) 
    {
        enemy.SetTarget(null);
        enemy.ResetMoveSpeed();
    }

}
