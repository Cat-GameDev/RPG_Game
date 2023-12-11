using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIGamePlay : UICanvas
{
    public Button[] buttons;
    Player player;
    private Action[] actions;

    void Start()
    {
        player = LevelManager.Instance.Player;

        actions = new Action[]
        {
            player.Jump,
            player.Dash,
            player.Attack,
            player.CounterAttack,
            player.ThrowAttack
        };

        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; 
            buttons[i].onClick.AddListener(() => actions[index].Invoke());
        }
    }
}