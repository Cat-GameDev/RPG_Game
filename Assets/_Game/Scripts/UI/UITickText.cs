using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITickText : GameUnit
{
    [SerializeField] TextMeshProUGUI text;
    Blackhole_Skill_Controller blackhole_Skill_Controller;

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
    }


}
