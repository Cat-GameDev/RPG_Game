using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill : Skill
{
    [Header("Skill infor")]
    [SerializeField] float throwSpeed;
    [SerializeField] float returnSpeed;

    public void CreateSword()
    {
        Sword_Skil_Controller sword_Skil_Controller =  SimplePool.Spawn<Sword_Skil_Controller>(PoolType.Sword, player.TF.position, player.TF.rotation);
        player.SetCurrentSword(sword_Skil_Controller);
        sword_Skil_Controller.OnInit();
        sword_Skil_Controller.SetupSword(throwSpeed, returnSpeed);
    }

}
