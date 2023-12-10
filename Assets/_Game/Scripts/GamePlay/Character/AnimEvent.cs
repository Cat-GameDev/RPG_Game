using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    [SerializeField] protected Character character;
    public void AttackOver()
    {
        character.AttackOver();
    }

    public void AttackStart()
    {
        character.AttackStart();
    }

    public void OnDespawn()
    {
        character.OnDespawn();
    }

}
