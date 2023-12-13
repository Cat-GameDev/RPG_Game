using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
};

public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Spin")]
    [SerializeField] float maxDistance;
    [SerializeField] float spinDuration;
    [SerializeField] float hitCooldown;

    [Header("Bounce")]
    [SerializeField] float bounceAmount;
    [SerializeField] float bounceSpeed;

    [Header("Pierce")]
    [SerializeField] float pierceAmount;

    [Header("Skill")]
    [SerializeField] float throwSpeed;
    [SerializeField] float returnSpeed;
    [SerializeField] float freezeTime;

    public void CreateSword()
    {
        Sword_Skil_Controller sword_Skil_Controller =  SimplePool.Spawn<Sword_Skil_Controller>(PoolType.Sword, player.TF.position, player.TF.rotation);
        sword_Skil_Controller.OnInit();

        switch (swordType)
        {
            case SwordType.Bounce:
                sword_Skil_Controller.SetUpBounce(true, bounceSpeed, bounceAmount);
                break;

            case SwordType.Pierce:
                sword_Skil_Controller.SetupPierce(pierceAmount);
                break;

            case SwordType.Spin:
                sword_Skil_Controller.SetupSpin(true, maxDistance, spinDuration, hitCooldown);
                break;

            default:
                // something else
                break;
        }


        player.SetCurrentSword(sword_Skil_Controller);
        
        sword_Skil_Controller.SetupSword(throwSpeed, returnSpeed, freezeTime);
    }

}
