
using UnityEngine;

public class GameUnit : MonoBehaviour
{
    private Transform tf;
    public Transform TF
    {
        get
        {
            tf = tf ?? gameObject.transform;
            // if (tf == null)
            // {
            //     tf = transform;
            // }
            return tf;
        }
    }

    public PoolType poolType;
    public virtual void OnDespawn() => SimplePool.Despawn(this);
}