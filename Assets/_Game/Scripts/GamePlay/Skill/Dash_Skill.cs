using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [SerializeField] float dashDistance;
    [SerializeField] float dashDuration;

    [Header("Dash")]
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;
    public bool dashUnlocked { get; private set; }

    [Header("Clone on dash")]
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockButton;
    public bool cloneOnDashUnlocked { get; private set; }

    [Header("Clone on arrival")]
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockButton;
    public bool cloneOnArrivalUnlocked { get; private set; }

    protected override void Start()
    {
        base.Start();
        
        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }

    public void DashSkill(Player player, StateMachine stateMachine, bool dir, bool canAttack, float damage)
    {
        Vector2 dashDirection = player.GetDirection(dir);
        Vector2 targetPosition = (Vector2)player.TF.position + dashDirection * dashDistance;

        Vector2 originalPosition = player.TF.position;

        Tweener dashTween = null;

        dashTween = DOTween.To(() => (Vector2)player.TF.position, x => player.TF.position = x, targetPosition, dashDuration)
            .SetEase(Ease.Linear)
            .OnStart(() =>
            {
                CloneOnDash(player.TF.position, dir, canAttack, damage);
            })
            .OnUpdate(() =>
            {
                if (player.isWallDetected())
                {
                    //isDashing = false;
                    dashTween.Kill(); 
                    stateMachine.ChangeState(player.IdleState);
                }
            })
            .OnComplete(() =>
            {
                //isDashing = false;
                player.TF.position = targetPosition;
                stateMachine.ChangeState(player.IdleState);
                CloneOnArrial(player.TF.position, dir, canAttack, damage);
            });
    }

    protected void CheckUnlock()
    {
        UnlockDash();
        UnlockCloneOnDash();
        UnlockCloneOnArrival();
    }

    private void UnlockDash()
    { 
        if (dashUnlockButton.unlocked)
            dashUnlocked = true;
    }

    private void UnlockCloneOnDash()
    {
        if (cloneOnDashUnlockButton.unlocked)
            cloneOnDashUnlocked = true;
    }

    private void UnlockCloneOnArrival()
    {
        if(cloneOnArrivalUnlockButton.unlocked)
            cloneOnArrivalUnlocked = true;
    }

    
    public void CloneOnDash(Vector3 position, bool isRight, bool canAttack, float damage)
    {
        if(cloneOnDashUnlocked)
        {
            SkillManager.Instance.Clone_Skill.CreateClone(position, isRight, canAttack, damage);
        }
    }

    public void CloneOnArrial(Vector3 position, bool isRight, bool canAttack, float damage)
    {
        if(cloneOnArrivalUnlocked)
        {
            SkillManager.Instance.Clone_Skill.CreateClone(position, isRight, canAttack, damage);
        }

    }



}