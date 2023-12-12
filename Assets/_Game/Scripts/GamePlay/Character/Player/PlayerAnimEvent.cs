using UnityEngine;

public class PlayerAnimEvent : AnimEvent
{
    [SerializeField] Player player;
    public void ThrowAttackOver() => player.ThrowAttackOver();
    public void ThrowSword() => SkillManager.Instance.Sword_Skill.CreateSword();
    public void CatchOver() => player.CatchOver();
}