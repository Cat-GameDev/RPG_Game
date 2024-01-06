using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Crystal_Skill : Skill
{
    public const float TIME_DELAY_CRYSTAL = 0.07f;
    Crystal_Skill_Controller crystal_Skill_Controller = null;
    [SerializeField] float crystalDuration; //TODO: increase Cooldown(In Skill) 2.5f if canMove
    [SerializeField] float moveSpeed;

    public bool canMultiCrystal;
    public bool canMove;
    public bool canExplode;

    

    public bool crystalUnlocked;
    [SerializeField] private UI_SkillTreeSlot unlockCrystalButton;
    [SerializeField] private UI_SkillTreeSlot unlockExplosiveButton;
    [SerializeField] private UI_SkillTreeSlot unlockMovingCrystalButton;
    [SerializeField] private UI_SkillTreeSlot unlockMultiStackButton;
    protected override void Start()
    {
        base.Start();
        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
        unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
        unlockMultiStackButton.GetComponent<Button>().onClick.AddListener(UnlockMultiStack);
    }

    #region Unlock skill region
    protected void CheckUnlock()
    {
        UnlockCrystal();
        UnlockExplosiveCrystal();
        UnlockMovingCrystal();
        UnlockMultiStack();


    }
    private void UnlockCrystal()
    {
        if (unlockCrystalButton.unlocked)
            crystalUnlocked = true;
    }

    private void UnlockExplosiveCrystal()
    {
        if (unlockExplosiveButton.unlocked)
        {
            canExplode = true;
            cooldown = crystalDuration;
        }
    }

    private void UnlockMovingCrystal()
    {
        if (unlockMovingCrystalButton.unlocked)
            canMove = true;
    }

    private void UnlockMultiStack()
    {
        if (unlockMovingCrystalButton.unlocked)
            canMultiCrystal = true;
    }

    #endregion 

    public override void UseSkill()
    {
        //base.UseSkill();
        if(canMultiCrystal)
        {
            StartCoroutine(SpawnCrystalsWithDelay());
            return;
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
            
            SelfDespawn();
        }

        
    }

    public void CreateCrystal()
    {
        if(!crystalUnlocked) return;
        
        if(crystal_Skill_Controller == null)
        {
            crystal_Skill_Controller = SimplePool.Spawn<Crystal_Skill_Controller>(PoolType.Crystal, player.TF.position, Quaternion.identity);
            crystal_Skill_Controller.OnInit(moveSpeed, canMove, player);
        }
        
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
            crystal_Skill.OnInit(moveSpeed, canMove, player);

            yield return new WaitForSeconds(TIME_DELAY_CRYSTAL);
        }
    }
}
