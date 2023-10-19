using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl_BossStage : PlayerControl_GamePlay
{
    protected override void Update()
    {
        if (!isEnabledControl || !isAvailableControl) return;

        if (!GetStats<PlayerStats>().hp.isAlive) return;

        GetAttack<PlayerAttack>().ResetAttackTargets();
        MoveCheck();

        UpdateRaycast();

        StartAttackCheck();

        base.Update();
    }

    protected override void KilledEnemy(EnemyControl enemyControl)
    {
        QuestDataEventManager.instance.PlayerKilledEnemy(enemyControl);
    }

    public void UpdateRaycast()
    {
        PlayerRaycast_BossStage raycast = pRaycast as PlayerRaycast_BossStage;

        Collider[] attackRange = raycast.attackRangeRaycast.GetRaycastHit();
        if (attackRange.Length > 0)
            AttackRange(attackRange);
    }

    public void StartIntro()
    {
        utility.weaponModel.SetWeaponModelType(PlayerWeaponModelType.Model);

        GetModel<Model>().animationControl.ResetAnimationState();
        GetModel<Model>().animationControl.PlayAnimation("BossAppear_Stand_Idle", isRepeat: true);
    }

    public void EndIntro()
    {
        GetModel<Model>().animationControl.ResetAnimationState();
        GetModel<Model>().animationControl.PlayAnimation("Idle_Weapon_On", isRepeat: true);
    }

    private void AttackRange(Collider[] toBeHitCols)
    {
        PlayerAttack_GamePlay attack = GetAttack<PlayerAttack_GamePlay>();

        attack.AddAttackTargets(toBeHitCols);
    }

    public void EndPlayer(Action OnComplete)
    {
        transform.position = Vector3.zero;
        transform.eulerAngles = new Vector3(0, 180, 0);

        GetAttack<PlayerAttack_GamePlay>().StopAttack();
        GetModel<PlayerModel>().animationControl.PlayAnimation("Finish_Blow_PC", weight: (int)PlayerAnimationWeight.FinishBlow, OnAnimationEnd: OnComplete);

        utility.subCamera.PlayCameraAnimation("Finish_Blow_Cam", DefineManager.DEFAULT_CAMERA_TIME, isReturnToMainCameraAfterEnd: false);
    }

    private void MoveCheck()
    {
        PlayerAttack attack = GetAttack<PlayerAttack>();

        if (attack.isUseSkill) return;

        if (utility.input.GetClickTime() <= 1.0f)
        {
            int horizontal_Drag = utility.input.GetHorizontalDrag(sensitivity: 0.1f);

            if (horizontal_Drag != 0)
            {
                GetAttack<PlayerAttack>().StopAttack();

                PlayerMove_BossStage move = GetMove<PlayerMove_BossStage>();
                switch (horizontal_Drag)
                {
                    case -1:
                        move.Move_Left();
                        return;
                    case 1:
                        move.Move_Right();
                        return;
                }
                return;
            }
        }
    }
}
