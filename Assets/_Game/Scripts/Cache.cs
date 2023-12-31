using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Cache
{
    private static Dictionary<Collider2D, IHit> ihits = new Dictionary<Collider2D, IHit>();

    public static IHit GetIHit(Collider2D collider)
    {
        if (!ihits.ContainsKey(collider))
        {
            ihits.Add(collider, collider.GetComponent<IHit>());
        }

        return ihits[collider];
    }

    private static Dictionary<Collider2D, Character> characters = new Dictionary<Collider2D, Character>();

    public static Character GetCharacter(Collider2D collider)
    {
        if (!characters.ContainsKey(collider))
        {
            characters.Add(collider, collider.GetComponent<Character>());
        }

        return characters[collider];
    }

    //Character playerCharacter = Cache.GetCharacter(other);

    private static Dictionary<Collider2D, Enemy> enemies = new Dictionary<Collider2D, Enemy>();

    public static Enemy GetEnemy(Collider2D collider)
    {
        if (!enemies.ContainsKey(collider))
        {
            enemies.Add(collider, collider.GetComponent<Enemy>());
        }

        return enemies[collider];
    }

    private static Dictionary<Collider2D, CharacterStats> characterStats = new Dictionary<Collider2D, CharacterStats>();

    public static CharacterStats GetCharacterStats(Collider2D collider)
    {
        if (!characterStats.ContainsKey(collider))
        {
            characterStats.Add(collider, collider.GetComponent<CharacterStats>());
        }

        return characterStats[collider];
    }

    
    private static Dictionary<Collider2D, Player> player = new Dictionary<Collider2D, Player>();

    public static Player GetPlayer(Collider2D collider)
    {
        if (!player.ContainsKey(collider))
        {
            player.Add(collider, collider.GetComponent<Player>());
        }

        return player[collider];
    }


}
