using System;
using UnityEngine;
using UnityEngine.Events;

public class UIGamePlay : UICanvas
{
    [SerializeField] UnityEvent OnJump, OnDash, OnAttack;

    void Update()
    {
        test();
    }
    public void JumpButton()
    {
        OnJump?.Invoke();
    }

    public void DashButton()
    {
        OnDash?.Invoke();
    }

    public void AttackButton()
    {
        OnAttack?.Invoke();
    }

    public void test()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            OnJump?.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            OnDash?.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.J))
        {
            OnAttack?.Invoke();
        }
    }
}