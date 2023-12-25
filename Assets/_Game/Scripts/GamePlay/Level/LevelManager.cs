using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] Player player;
    [SerializeField] List<Enemy> enemies = new List<Enemy>();

    public Player Player { get => player;}

    void Start()
    {
        player.OnInit();
        foreach(var enemy in enemies)
        {
            if(enemy != null)
            {
                enemy.OnInit();
            }
            
        }
    }
}