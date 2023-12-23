using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Clone_Skill_Controller : Character
{
    [SerializeField] SpriteRenderer sr;
    float cloneTimer;
    float cloneDuration;

    void Start()
    {
        characterStats = LevelManager.Instance.Player.characterStats;
    }

    public override void OnInit()
    {
        sr.color = new Color(1,1,1,1);
        DeActiveAttack();
    }
    void Update()
    {
        cloneTimer -= Time.deltaTime;
        if(cloneTimer < 0)
        {
            sr.color = new Color(1,1,1, sr.color.a - (Time.deltaTime * cloneDuration));

            if(sr.color.a < 0.5)
            {
                OnDespawn();
            }
        }

    }
    internal void SetupClone(Vector3 position, float cloneDuration, bool isRight, bool canAttack, float damage)
    {
        if(canAttack)
        {
            int random = Random.Range(1, 4);
            anim.SetInteger(Constants.ANIM_ATTACK, random);
        }
        TF.position = position;
        cloneTimer = cloneDuration;
        this.cloneDuration = cloneDuration;
        TF.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector2.up * 180f);
    }

}
