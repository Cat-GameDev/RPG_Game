using UnityEngine;

[CreateAssetMenu(fileName ="IceEffect", menuName = "Data/ItemEffect/IceEffect", order = 1)]
public class IceEffect : ItemEffect
{
    public override void ExecuteEffect(Transform target)
    {
        Fire_Controller fire_Controller = SimplePool.Spawn<Fire_Controller>
                                            (PoolType.Fire_Effect, target.position, Quaternion.identity);

        fire_Controller.OnInit();
    }
}