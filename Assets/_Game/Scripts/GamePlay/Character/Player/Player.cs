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
    //counter Attack

    public float counterAttackTimer;
    float counterAttackCooldown;
    [SerializeField] CounterAttackArea counterAttackArea;
    bool isSuccessfulCounterAttack;

    //ThrowAttack
    bool isThrowAttack;
    bool isCatched;
    Sword_Skil_Controller currentSword;
    Quaternion throwAttackRotation;

    void Start()
    {
        skill = SkillManager.Instance;
    }

    void Update()
    {
        if(IsDead || !GameManager.Instance.IsState(GameState.Gameplay))
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

        if(Input.GetKeyDown(KeyCode.O))
        {
            UltimateAttack();
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            if(skill.Crystal_Skill.CanUseSkill())
            {
                skill.Crystal_Skill.UseSkill();
            }
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        isJumping = isAttacking = isDashing = canAttack = isThrowAttack = isSuccessfulCounterAttack = isCatched = false;
        counterAttackArea.gameObject.SetActive(false);
        currentSword = null;
    }

    #region Ability Fuction
    public void UltimateAttack()
    {
        if(skill.Blackhole_Skill.CanUseSkill() && rb.velocity.y == 0)
        {
            stateMachine.ChangeState(UltimateAttackState);
        }
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
        if(isThrowAttack)
            return;

        if(IsGrounded())
            stateMachine.ChangeState(JumpState);
        else if(isWallDetected())
            stateMachine.ChangeState(JumpWallState);
    }

    public void Dash()
    {
        if(isThrowAttack)
            return;

        if(skill.Dash_Skill.CanUseSkill())
        {
            stateMachine.ChangeState(DashState);
        }
        
    }

    public void Attack()
    {

        if(!skill.Dash_Skill.CanUseSkill() && !isJumping && !(rb.velocity.y < 0) && !isAttacking && !isThrowAttack) 
        {
            stateMachine.ChangeState(PrimaryAttackState);
        }
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
    public void CounterAttack()
    {
        if(isThrowAttack)
            return;

        if(counterAttackTimer < counterAttackCooldown)
        {
            stateMachine.ChangeState(CounterAttackState);
            counterAttackCooldown = 0;
        }
        
    }

    public void ThrowAttack()
    {
        if(!currentSword)
        {
            stateMachine.ChangeState(ThrowAttackState);
        }
    }

    public void ThrowAttackOver() => isThrowAttack = false;
    public void CatchTheSword()
    {
        stateMachine.ChangeState(CatchState);
    }

    #endregion
    public void CatchOver() => isCatched = false;
    public void SetCurrentSword(Sword_Skil_Controller sword_Skil_Controller) => currentSword = sword_Skil_Controller;
    public bool isWallDetected()
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
    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(TF.position, Vector2.down, groundCheckDis, groundLayerMask);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                return true;
            }
        }
        return false;
    } 
    public void SetIsSuccessfulCounterAttack(bool isSuccessfulCounterAttack) => this.isSuccessfulCounterAttack = isSuccessfulCounterAttack;

    #region StateMachine
    public override void IdleState(ref Action onEnter, ref Action onExecute, ref Action onExit)
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
            
            skill.Dash_Skill.DashSkill(this, stateMachine, isRight);
            // Vector2 dashDirection = GetDirection(isRight);
            // Vector2 targetPosition = (Vector2)TF.position + dashDirection * dashDistance;

            // Vector2 originalPosition = TF.position;

            // Tweener dashTween = null;

            // dashTween = DOTween.To(() => (Vector2)TF.position, x => TF.position = x, targetPosition, dashDuration)
            //     .SetEase(Ease.Linear)
            //     .OnUpdate(() =>
            //     {
            //         if (isWallDetected())
            //         {
            //             isDashing = false;
            //             dashTween.Kill(); 
            //             stateMachine.ChangeState(IdleState);
            //         }
            //     })
            //     .OnComplete(() =>
            //     {
            //         isDashing = false;
            //         TF.position = targetPosition;
            //         stateMachine.ChangeState(IdleState);
            //     });
        };

        onExit = () =>
        {
            isDashing = false;
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

    private void ThrowAttackState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        onEnter = () =>
        {   
            //lưu lại rotation của Player
            throwAttackRotation = TF.rotation;
            ChangeAnim(Constants.ANIM_THROW_ATTACK);
            isThrowAttack = true;
            rb.velocity = Vector2.zero;
        };

        onExecute = () =>
        {
            if(!isThrowAttack)
            {
                stateMachine.ChangeState(IdleState);
            }
        };
    }

    private void CatchState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        onEnter = () =>
        {   
            //rotation của Player = đã lưu ở trên ThrowAttackState
            TF.rotation = throwAttackRotation;
            
            ChangeAnim(Constants.ANIM_CATCH);
            rb.velocity = Vector2.zero;
            isCatched = true;
            if (TF.rotation == Quaternion.Euler(Vector3.zero))
            {
                rb.velocity = new Vector2(-2f, rb.velocity.y);
            }
            else 
            {
                rb.velocity = new Vector2(2f, rb.velocity.y);
            }
            
        };

        onExecute = () =>
        {
            if(!isCatched)
            {
                stateMachine.ChangeState(IdleState);
                currentSword = null;
            }
        };

        onExit = () =>
        {
            
        };

    }

    private void UltimateAttackState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        float timeFly = .5f;
        float yPostion;
        onEnter = () =>
        {   
            GameManager.Instance.ChangeState(GameState.UltimateSkill);
            rb.gravityScale = 0;
            yPostion = transform.position.y;
            stateTimer = timeFly;
            ChangeAnim(Constants.ANIM_JUMP);

            // Use DOTween to tween the position
            transform.DOMoveY(transform.position.y + 5f, timeFly)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    //TODO: create blackhole
                    skill.Blackhole_Skill.UseSkill();
                    

                    // Fall tween
                    transform.DOMoveY(transform.position.y - .5f, Constants.TIME_ULTIMATE_SKILL)
                        .SetEase(Ease.InQuad)
                        .OnStart(() => 
                        {
                            //ChangeAnim(Constants.ANIM_FALL);
                            stateMachine.ChangeState(FallState);
                        })
                        .OnComplete(() => 
                        {
                            transform.DOMoveY(yPostion, 0.3f);
                            rb.gravityScale = 3;
                            GameManager.Instance.ChangeState(GameState.Gameplay);
                        });
                });
        };

        onExecute = () =>
        {

        };
        onExit = () =>
        {
            
        };
    }

    #endregion

    // private void State(ref Action onEnter, ref Action onExecute, ref Action onExit)
    // {
    //     onEnter = () =>
    //     {   

    //     };

    //     onExecute = () =>
    //     {
                
    //     };
    //     onExit = () =>
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
