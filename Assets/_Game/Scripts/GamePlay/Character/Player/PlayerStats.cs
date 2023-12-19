using UnityEngine;

public class PlayerStats : CharacterStats
{
    [SerializeField] Player player;
    public override void OnDeath()
    {
        player.OnDeath();
    }
}