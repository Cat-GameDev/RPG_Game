using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Crystal_Skill : Skill
{
    public const float TIME_DELAY_CRYSTAL = 0.07f;
    Crystal_Skill_Controller crystal_Skill_Controller;
    [SerializeField] float crystalDuration; //TODO: increase Cooldown(In Skill) 2.5f if canMove
    [SerializeField] float moveSpeed;

    
    public bool cloneInsteadOfCrystal;
    public bool canMultiCrystal;
    public bool canMove;
    public bool canExplode;

    public override void UseSkill()
    {
        base.UseSkill();
        if(canMultiCrystal)
        {
            StartCoroutine(SpawnCrystalsWithDelay());
            canMultiCrystal = false;
        }
        else if(crystal_Skill_Controller == null)
        {
            CreateCrystal();

            if (canExplode)
            {
                Invoke(nameof(SelfExplode), crystalDuration);
            }
            else
            {
                Invoke(nameof(SelfDespawn), crystalDuration);
            }

            //canMultiCrystal = true; để tạo ra 1 crytals, xog 3 crystal

        }
        else
        {
            if(canMove)
                return;
        
            Vector3 currentPositon = player.TF.position;
            player.TF.position = crystal_Skill_Controller.TF.position;
            crystal_Skill_Controller.TF.position = currentPositon;


            if(cloneInsteadOfCrystal)
            {
                SkillManager.Instance.Clone_Skill.CreateClone(currentPositon, player.IsRight, player.CanAttack, player.Damage);
            }
            
            SelfDespawn();
        }

        
    }

    public void CreateCrystal()
    {
        crystal_Skill_Controller = SimplePool.Spawn<Crystal_Skill_Controller>
                                                    (PoolType.Crystal, player.TF.position, Quaternion.identity);
        crystal_Skill_Controller.OnInit(moveSpeed, canMove, cloneInsteadOfCrystal);
    }

    private void SelfDespawn()
    {
        if(crystal_Skill_Controller != null)
        {
            crystal_Skill_Controller.OnDespawn();
            crystal_Skill_Controller = null;
        }
    }


    private void SelfExplode()
    {
        if(crystal_Skill_Controller != null && canMultiCrystal)
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
            crystal_Skill.OnInit(moveSpeed, canMove, cloneInsteadOfCrystal);

            yield return new WaitForSeconds(TIME_DELAY_CRYSTAL);
        }
    }
}
