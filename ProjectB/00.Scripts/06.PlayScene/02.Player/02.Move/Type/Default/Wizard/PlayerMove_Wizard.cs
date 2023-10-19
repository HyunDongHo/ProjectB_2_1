using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Scheduler;

public class PlayerMoveState_Wizard : PlayerMoveState_GamePlay
{
    public const string RUN = "RUN";
    public const string RUN_WAIT = "RUN_WAIT";
    public const string DASH = "DASH";
    public const string DASH_TO_TARGET = "DASH_TO_TARGET";
}

public class PlayerMove_Wizard : PlayerMove_DefaultStage
{
    public override void DefaultMoveUpdate()
    {
        MoveStateCheck();

        if (!isAvaliableUpdateMove) return;

        Run();
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


    private void RotateModelPivotToIdentity()
    {
        playerControl.utility.modelPivot.localRotation = Quaternion.identity;
    }

    //private float mapCircleBorderPlayerAngle = 0;

    protected override void MoveDefault(float speed)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, Mathf.Infinity, ~LayerMask.GetMask("Player") & ~LayerMask.GetMask("Ignore Raycast"));

        if (hits.Length > 0)
        {
            int index = Logic.GetNearestObjectIndex(transform.position, hits, "Enemy");

            if (index < 0)
                return;

            Collider nearestObject = hits[index];

            Vector3 reachPosition = nearestObject.ClosestPoint(transform.position);
            reachPosition.y = 0;

            transform.position = Vector3.MoveTowards(transform.position, reachPosition, speed * Time.deltaTime);
            RotateToTarget(nearestObject.transform.position);
        }
    }

    protected override void MoveToTarget(float speed, Collider target)
    {
        Vector3 reachPosition = target.ClosestPoint(transform.position);
        reachPosition.y = 0;

        transform.position = Vector3.MoveTowards(transform.position, reachPosition, speed * Time.deltaTime);
        RotateToTarget(target.transform.position);

        mapCircleBorderPlayerAngle = -(Quaternion.FromToRotation(Vector3.right, transform.position).eulerAngles.y - 360);
    }

    protected override void MoveStateCheck()
    {
        base.MoveStateCheck();
    }
}
