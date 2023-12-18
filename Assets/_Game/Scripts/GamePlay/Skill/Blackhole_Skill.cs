using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill : Skill
{
    [SerializeField] float maxSize;
    [SerializeField] float growSpeed;
    [SerializeField] int attackAmount;
    [SerializeField] int maxAttackAmount;
    public override void UseSkill()
    {
        if(SkillManager.Instance.Clone_Skill.CrystalInsteadOfClone)
        {
            attackAmount = maxAttackAmount;
        }
        Blackhole_Skill_Controller blackhole_Skill_Controller = SimplePool.Spawn<Blackhole_Skill_Controller>(PoolType.Blackhole, player.TF.position, player.TF.rotation);
        blackhole_Skill_Controller.Setup(maxSize, growSpeed, attackAmount);
        blackhole_Skill_Controller.OnInit();
    }
}
