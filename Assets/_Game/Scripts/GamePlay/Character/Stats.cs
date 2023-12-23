using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats 
{
    [SerializeField] int baseValue;
    public List<int> modifiers;
    public int GetValue()
    {
        int finalValue = baseValue;

        foreach(int modifier in modifiers)
        {
            finalValue += modifier;
        }

        return finalValue;
    }

    public void SetDefoutlValue(int value)
    {
        baseValue = value;
    }

    public void AddModifier(int modifiers)
    {
        this.modifiers.Add(modifiers);
    }

    public void RemoveModifier(int modifiers)
    {
        this.modifiers.Remove(modifiers);
    }
}
