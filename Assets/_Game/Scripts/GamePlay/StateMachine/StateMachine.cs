using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StateMachine
{
#if UNITY_EDITOR
    //cai nay de biet duoc no dang o state nao
    public string name;
#endif

    public delegate void StateAction(ref Action onEnter, ref Action onExecute, ref Action onExit);
    private Action onEnter, onExecute, onExit;

    public void Execute()
    {
        onExecute?.Invoke();
    }

//     public void ChangeState(StateAction stateAction)
//     {
//         onExit?.Invoke();
//         stateAction.Invoke(ref onEnter, ref onExecute, ref onExit);
//         onEnter?.Invoke();

// #if UNITY_EDITOR
//         //cai nay de biet duoc no dang o state nao
//         name = stateAction.Method.Name;
// #endif
//     }
    public void ChangeState(StateAction stateAction)
    {
        onExit?.Invoke();

        if (stateAction != null)
        {
            stateAction.Invoke(ref onEnter, ref onExecute, ref onExit);

    #if UNITY_EDITOR
            // Cai nay de biet duoc no dang o state nao
            name = stateAction.Method.Name;
    #endif
        }

        onEnter?.Invoke();
    }
}


//private void State(ref Action onEnter, ref Action onExecute, ref Action onExit)
//{
//    onEnter = () =>
//    {

//    };

//    onExecute = () =>
//    {

//    };

//    onExit = () =>
//    {

//    };
//}