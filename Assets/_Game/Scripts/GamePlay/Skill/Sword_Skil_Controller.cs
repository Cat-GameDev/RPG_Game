using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skil_Controller : GameUnit
{
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] CircleCollider2D circleCollider2D;
    [SerializeField] Player player;

    public void SetupSword(Vector2 dir, float gravityScale)
    {
        rb.velocity = dir;
        rb.gravityScale = gravityScale;
    }
}
