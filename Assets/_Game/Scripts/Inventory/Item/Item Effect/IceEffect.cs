using UnityEngine;

[CreateAssetMenu(fileName ="IceEffect", menuName = "Data/ItemEffect/IceEffect", order = 1)]
public class IceEffect : ItemEffect
{
    public override void ExecuteEffect(Transform target)
    {
        Ice_Controller ice_Controller = SimplePool.Spawn<Ice_Controller>
                                            (PoolType.Ice_Effect, target.position, Quaternion.identity);

        ice_Controller.OnInit();
    }
}