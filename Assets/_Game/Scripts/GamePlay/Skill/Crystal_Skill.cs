using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{
    public const float TIME_DELAY_CRYSTAL = 0.07f;
    Crystal_Skill_Controller crystal_Skill_Controller;
    [SerializeField] float crystalDuration;
    [SerializeField] float moveSpeed;
    

    public bool canMultiCrystal;
    public bool canMove;

    public override void UseSkill()
    {
        base.UseSkill();
        if(canMultiCrystal)
        {
            StartCoroutine(SpawnCrystalsWithDelay());
        }
        else if(crystal_Skill_Controller == null)
        {
            crystal_Skill_Controller = SimplePool.Spawn<Crystal_Skill_Controller>
                                            (PoolType.Crystal, player.TF.position, Quaternion.identity);
            crystal_Skill_Controller.OnInit(moveSpeed, canMove);  
            Invoke(nameof(SelfExplode), crystalDuration); 
            canMultiCrystal = true;
            //crystal_Skill_Controller.SetupCrystal(crystalDuration);
        }
        else
        {
            Vector3 currentPositon = player.TF.position;
            player.TF.position = crystal_Skill_Controller.TF.position;
            crystal_Skill_Controller.TF.position = currentPositon;
            //SelfExplode();
        }

        
    }

    private void SelfExplode()
    {
        if(crystal_Skill_Controller != null)
        {
            crystal_Skill_Controller.Explode();
            crystal_Skill_Controller = null;
        }
    }

    IEnumerator SpawnCrystalsWithDelay()
    {
        for (int i = 0; i < 3; i++)
        {
            Crystal_Skill_Controller crystal_Skill = SimplePool.Spawn<Crystal_Skill_Controller>(PoolType.Crystal, player.TF.position, Quaternion.identity);
            crystal_Skill.OnInit(moveSpeed, canMove);

            yield return new WaitForSeconds(TIME_DELAY_CRYSTAL);
        }
    }
}
