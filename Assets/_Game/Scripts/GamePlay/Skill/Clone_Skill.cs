using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] float cloneDuration;
    public void CreateClone(Vector3 position, bool isRight, bool canAttack, float damage)
    {
        Clone_Skill_Controller newClone = SimplePool.Spawn<Clone_Skill_Controller>(PoolType.Clone_Skill);
        newClone.SetupClone(position, cloneDuration, isRight, canAttack, damage);
    }
}
