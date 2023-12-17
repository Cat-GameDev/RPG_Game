using System;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : UICanvas
{
    public Button[] buttons;
    public GameObject panel;
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
            player.ThrowAttack,
            player.UltimateAttack,
        };

        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; 
            buttons[i].onClick.AddListener(() => actions[index].Invoke());
        }
    }

    void Update()
    {
        if(GameManager.Instance.IsState(GameState.UltimateSkill))
        {
            panel.SetActive(false);
        }
        else if(GameManager.Instance.IsState(GameState.Gameplay))
        {
            panel.SetActive(true);
        }
    }
}