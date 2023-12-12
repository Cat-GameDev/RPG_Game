using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Sword_Skil_Controller : GameUnit
{
    public const float TIME_RETURN_SWORD = 2f;
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] CircleCollider2D circleCollider2D;
    Player player;
    float returnSpeed;
    string currentAnim;
    bool isReturning;
    public float maxSpeed = 10f;

    void Start()
    {
        player = LevelManager.Instance.Player;
    }

    void Update()
    {
        if(isReturning)
        {
            TF.position = Vector2.Lerp(TF.position, player.TF.position, returnSpeed * Time.deltaTime);
            ChangeAnim("flip");
            if (Vector2.Distance(TF.position, player.TF.position) < 2f)
            {
                returnSpeed = returnSpeed + maxSpeed;
            }

            if (Vector2.Distance(TF.position, player.TF.position) < 1.5f)
            {
                ChangeAnim(Constants.ANIM_IDLE);
            }

            if(Vector2.Distance(TF.position, player.TF.position) < 1.1f)
            {
                player.CatchTheSword();
                OnDespawn();
                isReturning = false;
            }
        }

    }

    public void OnInit()
    {
        TF.parent = null;
        circleCollider2D.enabled = true;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints2D.None;
        isReturning = false;
    }


    public void SetupSword(float throwSpeed, float returnSpeed)
    {
        rb.velocity = transform.right * throwSpeed;
        this.returnSpeed = returnSpeed;
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

    public void ReturnSword()
    {
        rb.isKinematic = false;
        isReturning = true;
        TF.parent = null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        
        circleCollider2D.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        TF.parent = other.transform;
        Invoke(nameof(ReturnSword), TIME_RETURN_SWORD);
    }
}
