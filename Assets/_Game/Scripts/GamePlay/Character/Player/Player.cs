using System;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class Player : Character
{
    public const int ATTACK_COMBO = 2;
    public const float TIME_RESET_ATTACK = 2f;
    public const float MOVE_ATTACK = 0.1f;
    public const float COUNTER_ATTACK_DURATION = 0.2f;
    [SerializeField] FixedJoystick joystickControl;
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
    bool canAttack;
    int comboCounter;
    SkillManager skill;
    //aimSwrod
    bool isAimSword;
    public float counterAttackTimer;
    float counterAttackCooldown;
    [SerializeField] CounterAttackArea counterAttackArea;
    bool isSuccessfulCounterAttack;


    void Start()
    {
        skill = SkillManager.Instance;
    }

    void Update()
    {
        if(IsDead)
            return;
        
        comboTimeWidow -= Time.deltaTime;
        counterAttackCooldown += Time.deltaTime;
        stateMachine?.Execute();
        //Debug.Log(stateMachine.name);
        horizontalInput = joystickControl.Horizontal;
        verticaleInput = joystickControl.Vertical;

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticaleInput = Input.GetAxisRaw("Vertical");


        if(isWallDetected() && !IsGrounded() && !isJumping)
            stateMachine.ChangeState(WallSildeState);

        if(comboTimeWidow < 0)
        {
            canAttack = false;
            comboCounter = 0;
        }

        CheckInput();
        
    }

    private void CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if(Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            Dash();
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            CounterAttack();
        }

        if(Input.GetKeyDown(KeyCode.U))
        {
            ThrowAttack();
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        isJumping = isAttacking = isDashing = canAttack = isAimSword = isSuccessfulCounterAttack = false;
        counterAttackArea.gameObject.SetActive(false);
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
        if(IsGrounded())
            stateMachine.ChangeState(JumpState);
        else if(isWallDetected())
            stateMachine.ChangeState(JumpWallState);
    }

    public void Dash()
    {
        if(skill.Dash_Skill.CanUseSkill())
        {
            stateMachine.ChangeState(DashState);
        }
        
    }

    public void Attack()
    {

        if(!isDashing && !isJumping && !(rb.velocity.y < 0) && !isAttacking)
        {
            stateMachine.ChangeState(PrimaryAttackState);
        }
    }

    public void CounterAttack()
    {
        if(counterAttackTimer < counterAttackCooldown)
        {
            stateMachine.ChangeState(CounterAttackState);
            counterAttackCooldown = 0;
        }
        
    }

    public void ThrowAttack()
    {

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

    public void SetIsSuccessfulCounterAttack(bool isSuccessfulCounterAttack)
    {
        this.isSuccessfulCounterAttack = isSuccessfulCounterAttack;
    }

    public override void AttackOver()
    {
        base.AttackOver();
        isAttacking = false;
        isSuccessfulCounterAttack = false;
        comboCounter++;

        if(comboCounter > ATTACK_COMBO)
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
        onEnter = () =>
        {   
            ChangeAnim(Constants.ANIM_ATTACK);
            anim.SetInteger("comboCounter", comboCounter);
            comboTimeWidow = comboTime;
            isAttacking = true;
            rb.velocity = Vector2.zero;
            TF.position = new Vector2(TF.position.x + MOVE_ATTACK * GetDirection(isRight).x, TF.position.y);
            stateTimer = 0f;
            canAttack = true;
        };

        onExecute = () =>
        {
            stateTimer += Time.deltaTime;
            if(!isAttacking || stateTimer > TIME_RESET_ATTACK)
            {
                isAttacking = false;
                stateMachine.ChangeState(IdleState);
            }
                
        };

        onExit = () =>
        {
            isAttacking = false;
            DeActiveAttack();
        };
    }
    
    private void AimSwordState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        // float timerReset = 5f;
        // float aimSwordCooldown = 0f;
        // onEnter = () =>
        // {   
        //     ChangeAnim(Constants.ANIM_AIMSWORD);
        //     isAimSword = true;
        //     timerReset = 5f;
        //     aimSwordCooldown = 0f;
        // };

        // onExecute = () =>
        // {
        //     aimSwordCooldown += Time.deltaTime;
        //     if(aimSwordCooldown > timerReset)
        //     {
        //         throwSwordJoystick.ResetJoystick();
        //         aimSwordCooldown = 0;
        //         eventSystem.enabled = false;
        //         stateMachine.ChangeState(IdleState);
        //     }
        //     else if (Mathf.Abs(throwSwordJoystick.Horizontal) > 0.1f || Mathf.Abs(throwSwordJoystick.Vertical) > 0.1f)
        //     {
        //         return;
        //     }
        //     else
        //     {
        //         ChangeAnim(Constants.ANIM_THROWSWORD);
        //         if(!isAimSword)
        //             stateMachine.ChangeState(IdleState);
        //     }
        // };
    }

    private void CounterAttackState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        onEnter = () =>
        {   
            ChangeAnim(Constants.ANIM_COUNTER_ATTACK);
            rb.velocity = Vector2.zero;
            stateTimer = 0;
            counterAttackArea.gameObject.SetActive(true);
        };

        onExecute = () =>
        {
            stateTimer += Time.deltaTime;
            if(isSuccessfulCounterAttack)
            {
                stateMachine.ChangeState(SuccesfulCounterAttackState);
            }
            else
            {
                if(stateTimer > COUNTER_ATTACK_DURATION)
                {
                    stateMachine.ChangeState(IdleState);
                }
            }

        };

        onExit = () =>
        {
            counterAttackArea.gameObject.SetActive(false);
        };
    }

    private void SuccesfulCounterAttackState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        onEnter = () =>
        {   
            ChangeAnim(Constants.ANIM_SUCCESSFUL_COUNTER_ATTACK);
        };

        onExecute = () =>
        {
            if(!isSuccessfulCounterAttack)
            {
                stateMachine.ChangeState(IdleState);
            }
        };
    }

    // private void State(ref Action onEnter, ref Action onExecute, ref Action onExit)
    // {
    //     onEnter = () =>
    //     {   

    //     };

    //     onExecute = () =>
    //     {
                
    //     };
    // }



    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawLine(TF.position, TF.position + Vector3.down * groundCheckDis);
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }



}
