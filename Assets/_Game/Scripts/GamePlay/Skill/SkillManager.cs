using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    [SerializeField] Dash_Skill dash_Skill;
    [SerializeField] Clone_Skill clone_Skill;
    [SerializeField] Sword_Skill sword_Skill;
    [SerializeField] Blackhole_Skill blackhole_Skill;
    [SerializeField] Crystal_Skill crystal_Skill;
    public Dash_Skill Dash_Skill { get => dash_Skill;  }
    public Clone_Skill Clone_Skill { get => clone_Skill; }
    public Sword_Skill Sword_Skill { get => sword_Skill; }
    public Blackhole_Skill Blackhole_Skill { get => blackhole_Skill; }
    public Crystal_Skill Crystal_Skill { get => crystal_Skill;}
}