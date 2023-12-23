using UnityEngine;

public class ShockStrike_Controller : GameUnit
{
    public const float TIME_DESPAWN = 0.4f;
    public const float TIME_SELF_DESPAWN = 0.2f;
    CharacterStats targetStats;
    [SerializeField] private float speed;
    private int damage;

    [SerializeField] Animator anim;
    private bool triggered;
    string currentAnim;

    public void Oninit()
    {
        ChangeAnim(Constants.ANIM_IDLE);
        triggered = false;
        anim.transform.localPosition = Vector3.zero;
        anim.transform.localRotation = Quaternion.Euler(0, 0, 90f);

        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    public void Setup(int _damage, CharacterStats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
        Oninit();
    }

 
    void Update()
    {
        if(!targetStats)
            return;


        if (triggered)
            return;


        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;

        if (Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
        {
            anim.transform.localPosition = new Vector3(0, .5f);
            anim.transform.localRotation = Quaternion.identity;

            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(2.5f, 2.5f);


            Invoke(nameof(DamageAndSelfDespawn), TIME_SELF_DESPAWN);

            triggered = true;
            ChangeAnim(Constants.ANIM_ATTACK);
        }
    }

    private void DamageAndSelfDespawn()
    {
        targetStats.ApplyShock(true);
        targetStats.OnHit(damage);
        Invoke(nameof(OnDespawn), TIME_DESPAWN);
    }

    public void ChangeAnim(string animName)
    {
        if(currentAnim != animName)
        {
            anim.ResetTrigger(currentAnim);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
        }
    }
    
}
