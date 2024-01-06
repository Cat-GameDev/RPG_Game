using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] float cloneDuration;
    [SerializeField] bool canCreateCloneCounterAttack;
    [SerializeField] bool canDuplicateClone;
    [SerializeField] bool crystalInsteadOfClone;

    public bool CanDuplicateClone { get => canDuplicateClone;}
    public bool CrystalInsteadOfClone { get => crystalInsteadOfClone;  }

    public void CreateCloneCounterAttack(Vector3 position, bool isRight, bool canAttack, float damage)
    {
        if(canCreateCloneCounterAttack)
        {
            CreateClone(position, isRight, canAttack, damage);
        }
    }

    public void CreateClone(Vector3 position, bool isRight, bool canAttack, float damage)
    {
        if(crystalInsteadOfClone)
        {
            SkillManager.Instance.Crystal_Skill.CreateCrystal();
            return;
        }
        Clone_Skill_Controller newClone = SimplePool.Spawn<Clone_Skill_Controller>(PoolType.Clone_Skill);
        newClone.OnInit();
        newClone.SetupClone(position, cloneDuration, isRight, canAttack, damage);
    }
}
