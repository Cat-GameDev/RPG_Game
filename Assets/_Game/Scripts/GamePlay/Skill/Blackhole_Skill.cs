using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill : Skill
{
    [SerializeField] float maxSize;
    [SerializeField] float growSpeed;
    public override void UseSkill()
    {
        Blackhole_Skill_Controller blackhole_Skill_Controller = SimplePool.Spawn<Blackhole_Skill_Controller>(PoolType.Blackhole, player.TF.position, player.TF.rotation);
        blackhole_Skill_Controller.Setup(maxSize, growSpeed);
        blackhole_Skill_Controller.OnInit();
    }
}
