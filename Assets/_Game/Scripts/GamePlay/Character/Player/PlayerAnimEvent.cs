using UnityEngine;

public class PlayerAnimEvent : AnimEvent
{
    [SerializeField] Player player;
    public void AimSwordOver()
    {
        player.AimSwordOver();
    }

    public void ThrowSword()
    {
        SkillManager.Instance.Sword_Skill.CreateSword();
    }
}