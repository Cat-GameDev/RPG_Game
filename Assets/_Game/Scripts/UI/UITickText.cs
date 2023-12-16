using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITickText : GameUnit
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] RectTransform rectTransform;
    Vector3 offset;

    public void OnInit()
    {
        Invoke(nameof(OnDespawn), 3f);
    }

    internal void SetupText(Vector3 textTransform, Vector2 sizeUI, Vector3 offset)
    {
        text.transform.position = textTransform;
        rectTransform.sizeDelta = sizeUI;
        this.offset = offset;
    }

    public void UseSkill()
    {
        Invoke(nameof(CreateCloneRight), 2f);
        Invoke(nameof(CreateCloneLeft), 2.3f);
    }

    private void CreateCloneLeft()
    {
        SkillManager.Instance.Clone_Skill.CreateClone(TF.position + offset, false, true, 10f);
    }

    private void CreateCloneRight()
    {
        SkillManager.Instance.Clone_Skill.CreateClone(TF.position + new Vector3(-offset.x, offset.y,0), true, true, 10f);
    }
}
