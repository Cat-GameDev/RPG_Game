using UnityEngine;

[CreateAssetMenu(fileName ="FireEffect", menuName = "Data/ItemEffect/FireEffect", order = 1)]
public class FireEffect : ItemEffect
{
    public override void ExecuteEffect(Transform target)
    {
        Fire_Controller fire_Controller = SimplePool.Spawn<Fire_Controller>
                                            (PoolType.Fire_Effect, target.position, Quaternion.identity);

        fire_Controller.OnInit();
    }
}