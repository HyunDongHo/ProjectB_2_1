using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Scheduler;

public class PlayerMoveState_DefaultStage : PlayerMoveState_GamePlay
{
    public const string RUN = "RUN";
    public const string RUN_WAIT = "RUN_WAIT";
    public const string DASH = "DASH";
    public const string DASH_TO_TARGET = "DASH_TO_TARGET";
}

public abstract class PlayerMove_DefaultStage : PlayerMove_GamePlay
{
    public float runSpeed = 5;
    public float dashSpeed = 10;
    public float dashToTargetSpeed = 20;
    public float rotationSpeed = 0.5f;

    public float identityRotationSpeed = 0.05f;

    public float mapCircleBorderPlayerAngle = 0;
    public float mapCircleBorder = 83;

    public virtual void DefaultMoveUpdate()
    {
        MoveStateCheck();

        if (!isAvaliableUpdateMove || isNowBound == true) 
            return;

        if (CheckState(PlayerMoveState_DefaultStage.RUN))
        {
            Run();

        }
        else if (CheckState(PlayerMoveState_DefaultStage.DASH))
        {
            Dash();
        }
        else
        {
            Run();
        }
    }

    public void DefaultIdle()
    {
        playerControl.GetModel<PlayerModel>().PlayIdleAnimation(PlayerIdleType.Weapon_On);
    }
    public void RunToTargetUpdate(Collider target)
    {
        MoveStateCheck();

        if (!isAvaliableUpdateMove) return;
        RotateToTarget(target.transform.position);
        Run();
    }

    public void DashToTargetUpdate(Collider target)
    {
        MoveStateCheck();

        if (!isAvaliableUpdateMove) return;

        DashToTarget(target);
    }

    private void Run()
    {
        ChangeState(PlayerMoveState_DefaultStage.RUN);

        if (isAvailableMove)
        {
            RotateModelPivotToIdentity();
            playerControl.GetModel<PlayerModel>().PlayRunAnimation(fadeLength: 0.2f);

            MoveDefault(CalculateMoveSpeed(runSpeed));
        }
    }

    private void Dash()
    {
        ChangeState(PlayerMoveState_DefaultStage.DASH);

        if (isAvailableMove)
        {
            playerControl.GetModel<PlayerModel>().PlayDashAnimation();

            MoveDefault(CalculateMoveSpeed(dashSpeed));
        }
    }

    private void DashToTarget(Collider target)
    {
        ChangeState(PlayerMoveState_DefaultStage.DASH_TO_TARGET);

        if (isAvailableMove)
        {
            //RotateModelPivotToIdentity();
            playerControl.GetModel<PlayerModel>().PlayDashToTargetAnimation();

            MoveToTarget(CalculateMoveSpeed(dashToTargetSpeed), target);
        }
    }
    public void RotateToTarget(Vector3 targetPosition)
    {
        if (targetPosition - transform.position == Vector3.zero) return;

        Vector3 lookTargetEulerAngles = Quaternion.FromToRotation(Vector3.forward, (targetPosition - transform.position).normalized).eulerAngles;
        playerControl.utility.modelPivot.transform.eulerAngles = new Vector3(0, lookTargetEulerAngles.y, 0);
    }

    private void RotateModelPivotToIdentity()
    {
        playerControl.utility.modelPivot.localRotation = Quaternion.identity;
    }

    public float GetMapCircleBorderPlayerAngle()
    {
        return mapCircleBorderPlayerAngle;
    }

    protected abstract void MoveDefault(float speed);
    protected abstract void MoveToTarget(float speed, Collider target);

    protected virtual void MoveStateCheck()
    {
        if (CheckState(MoveState.STOP) && isAvailableMove)
        {
            playerControl.utility.weaponModel.SetWeaponModelType(PlayerWeaponModelType.Model);
        }
        else if (CheckState(PlayerMoveState_DefaultStage.DASH) && isAvailableMove)
        {
            if (playerControl.utility.weaponModel.GetWeaponActive())
            {
                PlayerModel model = playerControl.GetModel<PlayerModel>();
                model.PlayRunToDash(
                    OnFrame: (frame) =>
                    {
                        if (frame == 5)
                            playerControl.utility.weaponModel.SetWeaponModelType(PlayerWeaponModelType.Base);

                        if (frame == 12)
                            model.ResetAnimationState();
                    });
            }
        }
    }
}
