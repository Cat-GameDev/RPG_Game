using UnityEngine;

[CreateAssetMenu(fileName ="ItemEffect", menuName = "Data/ItemEffect", order = 1)]
public class ItemEffect : ScriptableObject
{
    public virtual void ExecuteEffect(Transform target)
    {

    }
}