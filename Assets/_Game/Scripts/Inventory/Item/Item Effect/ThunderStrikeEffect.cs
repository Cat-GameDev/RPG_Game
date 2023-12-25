using UnityEngine;

[CreateAssetMenu(fileName ="ThunderStrikeEffect", menuName = "Data/ItemEffect/ThunderStrikeEffect", order = 1)]
public class ThunderStrikeEffect : ItemEffect
{
    public override void ExecuteEffect(Transform target)
    {
        ThunderStrike_Controller thunderStrike_Controller = SimplePool.Spawn<ThunderStrike_Controller>
                                            (PoolType.ThunderStrike_Effect, target.position, Quaternion.identity);

        thunderStrike_Controller.OnInit();
    }
}