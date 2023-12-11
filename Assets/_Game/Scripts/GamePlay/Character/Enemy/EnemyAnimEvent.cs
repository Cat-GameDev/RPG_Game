using UnityEngine;

public class EnemyAnimEvent : AnimEvent
{
    [SerializeField] Enemy enemy;
    public void OpenCounterAttackWindow() =>  enemy.OpenCounterAttackWindow();
    public void CloseCounterAttackWindow() => enemy.CloseCounterAttackWindow();
}