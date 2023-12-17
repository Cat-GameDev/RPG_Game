using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITickText : GameUnit
{
    [SerializeField] TextMeshProUGUI text;
    Blackhole_Skill_Controller blackhole_Skill_Controller;
    //Vector3 offset;

    void Update()
    {
        if(blackhole_Skill_Controller != null && blackhole_Skill_Controller.CanAttack)
        {
            OnDespawn();
        }
    }

    internal void SetupText(Vector3 textTransform, Blackhole_Skill_Controller blackhole_Skill_Controller)
    {
        text.transform.position = textTransform;
        this.blackhole_Skill_Controller = blackhole_Skill_Controller;
        //SeftDespawn();
    }

    private void SeftDespawn()
    {
        Invoke(nameof(OnDespawn), 2f);
    }


    // private void CreateCloneLeft()
    // {
    //     SkillManager.Instance.Clone_Skill.CreateClone(TF.position + offset, false, true, 10f);
    // }

    // private void CreateCloneRight()
    // {
    //     SkillManager.Instance.Clone_Skill.CreateClone(TF.position + new Vector3(-offset.x, offset.y,0), true, true, 10f);
    // }
}
