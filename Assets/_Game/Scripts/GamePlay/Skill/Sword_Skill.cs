using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill : Skill
{
    [Header("Skill infor")]
    [SerializeField] Vector2 launchDir;
    [SerializeField] float swordGravity;
    Vector2 finalDir;

    public void CreateSword()
    {
        Sword_Skil_Controller sword_Skil_Controller =  SimplePool.Spawn<Sword_Skil_Controller>(PoolType.Sword, player.TF.position, player.TF.rotation);
        sword_Skil_Controller.SetupSword(launchDir, swordGravity);
    }

    // public Vector2 AimDir()
    // {
    //     Vector2 playerPosition = player.TF.position;

    // }
}
