using UnityEngine;
using DG.Tweening;

public class Dash_Skill : Skill
{
    [SerializeField] float dashDistance;
    [SerializeField] float dashDuration;

    public void DashSkill(Player player, StateMachine stateMachine, bool dir, bool canAttack, float damage)
    {
        Vector2 dashDirection = player.GetDirection(dir);
        Vector2 targetPosition = (Vector2)player.TF.position + dashDirection * dashDistance;

        Vector2 originalPosition = player.TF.position;

        Tweener dashTween = null;

        dashTween = DOTween.To(() => (Vector2)player.TF.position, x => player.TF.position = x, targetPosition, dashDuration)
            .SetEase(Ease.Linear)
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
                SkillManager.Instance.Clone_Skill.CreateOverClone(player.TF.position, dir, canAttack, damage);
            });
    }
}