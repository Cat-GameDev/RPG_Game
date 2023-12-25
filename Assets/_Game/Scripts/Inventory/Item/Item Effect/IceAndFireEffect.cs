using UnityEngine;

[CreateAssetMenu(fileName ="IceAndFireEffect", menuName = "Data/ItemEffect/IceAndFireEffect", order = 1)]
public class IceAndFireEffect : ItemEffect
{
    [SerializeField] float moveSpeed;
    public override void ExecuteEffect(Transform target)
    {
        Player player = LevelManager.Instance.Player;
        bool isThirAttack = player.ComboCounter == 2;
        
        if(isThirAttack)
        {
            IceAndFire_Controller iceAndFire_Controller = SimplePool.Spawn<IceAndFire_Controller>
                                            (PoolType.IceAndFire_Effect, target.position, player.TF.rotation);

            iceAndFire_Controller.OnInit(moveSpeed);
        }
        
    }
}