using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using Unity.Burst.CompilerServices;
using UnityEngine.EventSystems;

public class Player : Character
{
    [SerializeField] FixedJoystick joystickControl;
    [SerializeField] FixedJoystick throwSwordJoystick;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] float groundCheckDis;

    [Header("Dash")]
    [SerializeField] float dashDistance;
    [SerializeField] float dashDuration;

    [Header("Wall Check")]
    [SerializeField] Transform wallCheck;
    [SerializeField] Vector2 wallCheckSize;
    [SerializeField] float wallSildeSpeed;
    float horizontalInput;
    float verticaleInput;
    bool isJumping;
    bool isDashing;

    [Header("Attack")]
    [SerializeField] float comboTime;
    float comboTimeWidow;
    bool isAttacking;
    public bool canAttack;
    int comboCounter;
    SkillManager skill;
    //aimSwrod
    bool isAimSword;

    [SerializeField] EventSystem eventSystem;


    void Start()
    {
        skill = SkillManager.Instance;
    }

    void Update()
    {
        comboTimeWidow -= Time.deltaTime;

        stateMachine?.Execute();
        //Debug.Log(stateMachine.name);
        horizontalInput = joystickControl.Horizontal;
        verticaleInput = joystickControl.Vertical;

        // Check for joystick input to trigger AimSwordState
         if ((Mathf.Abs(throwSwordJoystick.Horizontal) > 0.1f || Mathf.Abs(throwSwordJoystick.Vertical) > 0.1f) && !isAimSword)
        {
            stateMachine.ChangeState(AimSwordState);
            return;
        }

        if(isWallDetected() && !IsGrounded() && !isJumping)
            stateMachine.ChangeState(WallSildeState);

        if(comboTimeWidow < 0)
        {
            canAttack = false;
            comboCounter = 0;
        }

        
    }

    public override void OnInit()
    {
        base.OnInit();
        isJumping = isAttacking = isDashing = canAttack = isAimSword = false;
    }

    public void ActiveEvenSystem()
    {
        eventSystem.enabled = true;
        AimSwordOver();
    }

    private void Moving()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // Flip Player
        if (horizontalInput > 0)
        {
            TF.rotation = Quaternion.Euler(Vector3.zero);
            isRight = true;
        }
        else if (horizontalInput < 0)
        {
            TF.rotation = Quaternion.Euler(0, 180, 0);
            isRight = false;
        }
    }
    
    public void Jump()
    {
        if(isAimSword || IsDead)
            return;

        if(IsGrounded())
            stateMachine.ChangeState(JumpState);
        else if(isWallDetected())
            stateMachine.ChangeState(JumpWallState);
    }

    public void Dash()
    {
        if(isAimSword || IsDead)
            return;

        if(skill.Dash_Skill.CanUseSkill())
        {
            stateMachine.ChangeState(DashState);
        }
        
    }

    public void Attack()
    {
        if(isAimSword || IsDead)
            return;

        if(!isDashing && !isJumping && !(rb.velocity.y < 0) && !isAttacking)
        {
            stateMachine.ChangeState(PrimaryAttackState);
        }
    }

    private bool isWallDetected()
    {
        RaycastHit2D hit = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, groundLayerMask);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                return true;
            }
        }
        return false;
    }
    private bool IsGrounded() => Physics2D.Raycast(TF.position, Vector2.down, groundCheckDis, groundLayerMask);


    public override void AttackOver()
    {
        base.AttackOver();
        isAttacking = false;
        comboCounter++;

        if(comboCounter > 2)
            comboCounter = 0;
    }
    public void AimSwordOver()
    {
        isAimSword = false;
    }

    protected override void IdleState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        onEnter = () =>
        {
            if(eventSystem.enabled == false)
            {
                Invoke(nameof(ActiveEvenSystem), 0.2f);
            }
            ChangeAnim(Constants.ANIM_IDLE);
            rb.velocity = Vector2.zero;
        };

        onExecute = () =>
        {
            if(!IsGrounded() && rb.velocity.y < 0)
            {   
                stateMachine.ChangeState(FallState);
            }
            else if(horizontalInput != 0)
            {
                stateMachine.ChangeState(MoveState);
            }

        };
    }

    private void MoveState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        onEnter = () =>
        {
            ChangeAnim(Constants.ANIM_RUN);
        };

        onExecute = () =>
        {
            Moving();

            if(!IsGrounded() && rb.velocity.y < 0)
            {   
                stateMachine.ChangeState(FallState);
            }
            if(horizontalInput == 0)
            {
                stateMachine.ChangeState(IdleState);
            }
        };
    }

    private void JumpState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        onEnter = () =>
        {
            ChangeAnim(Constants.ANIM_JUMP);
            isJumping = true;
            rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
        };

        onExecute = () =>
        {
            if(rb.velocity.y < 0)
                stateMachine.ChangeState(FallState);
        };
    }

    private void FallState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        onEnter = () =>
        {   
            ChangeAnim(Constants.ANIM_FALL);
            isJumping = false;
        };

        onExecute = () =>
        {
            if(IsGrounded())
                stateMachine.ChangeState(IdleState);
            else if(horizontalInput != 0)
                Moving();
            
        };
    }

    private void DashState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        onEnter = () =>
        {
            skill.Clone_Skill.CreateClone(TF.position, isRight, canAttack, damage);

            ChangeAnim(Constants.ANIM_DASH);
            isDashing = true;
            Vector2 dashDirection = GetDirection(isRight);
            Vector2 targetPosition = (Vector2)TF.position + dashDirection * dashDistance;

            Vector2 originalPosition = TF.position;

            Tweener dashTween = null;

            dashTween = DOTween.To(() => (Vector2)TF.position, x => TF.position = x, targetPosition, dashDuration)
                .SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    if (isWallDetected())
                    {
                        isDashing = false;
                        dashTween.Kill(); 
                        stateMachine.ChangeState(IdleState);
                    }
                })
                .OnComplete(() =>
                {
                    isDashing = false;
                    TF.position = targetPosition;
                    stateMachine.ChangeState(IdleState);
                });
        };
    }

    private void WallSildeState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        onEnter = () =>
        {   
            ChangeAnim(Constants.ANIM_WALLSILDE);
        };

        onExecute = () =>
        {
            if(verticaleInput < 0)
                rb.velocity = new Vector2(0, rb.velocity.y);
            else
                rb.velocity = new Vector2(0, rb.velocity.y * wallSildeSpeed);
            
            if(IsGrounded() || !isWallDetected())
                stateMachine.ChangeState(IdleState);
            
        };
    }

    private void JumpWallState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        onEnter = () =>
        {   
            Vector2 direction = GetDirection(!isRight);
            ChangeAnim(Constants.ANIM_JUMP);
            isJumping = true;
            rb.velocity = new Vector2(moveSpeed * direction.x, jumpForce);
            
            // Flip Player
            isRight = !isRight;
            if (isRight)
                TF.rotation = Quaternion.Euler(Vector3.zero);
            else
                TF.rotation = Quaternion.Euler(0, 180, 0);
            
        };

        onExecute = () =>
        {
            if(rb.velocity.y < 0)
                stateMachine.ChangeState(FallState);
            
        };
    }
   
    private void PrimaryAttackState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        float timerReset = 2f;
        float attackCooldown = 0f;
        onEnter = () =>
        {   
            ChangeAnim(Constants.ANIM_ATTACK);
            anim.SetInteger("comboCounter", comboCounter);
            comboTimeWidow = comboTime;
            isAttacking = true;
            rb.velocity = Vector2.zero;
            TF.position = new Vector2(TF.position.x + 0.1f * GetDirection(isRight).x, TF.position.y);
            timerReset = 2f;
            attackCooldown = 0f;
            canAttack = true;
        };

        onExecute = () =>
        {
            attackCooldown += Time.deltaTime;
            if(!isAttacking || attackCooldown > timerReset)
            {
                isAttacking = false;
                stateMachine.ChangeState(IdleState);
                attackCooldown = 0f;
            }
                
        };
    }
    
    private void AimSwordState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        float timerReset = 5f;
        float aimSwordCooldown = 0f;
        onEnter = () =>
        {   
            ChangeAnim(Constants.ANIM_AIMSWORD);
            isAimSword = true;
            timerReset = 5f;
            aimSwordCooldown = 0f;
        };

        onExecute = () =>
        {
            aimSwordCooldown += Time.deltaTime;
            if(aimSwordCooldown > timerReset)
            {
                throwSwordJoystick.ResetJoystick();
                aimSwordCooldown = 0;
                eventSystem.enabled = false;
                stateMachine.ChangeState(IdleState);
            }
            else if (Mathf.Abs(throwSwordJoystick.Horizontal) > 0.1f || Mathf.Abs(throwSwordJoystick.Vertical) > 0.1f)
            {
                return;
            }
            else
            {
                ChangeAnim(Constants.ANIM_THROWSWORD);
                if(!isAimSword)
                    stateMachine.ChangeState(IdleState);
            }
        };
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawLine(TF.position, TF.position + Vector3.down * groundCheckDis);
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }



}
