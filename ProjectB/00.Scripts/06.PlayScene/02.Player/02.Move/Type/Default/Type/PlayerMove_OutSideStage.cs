using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Scheduler;

public class PlayerMove_OutSideStage : PlayerMove_DefaultStage
{
    private float mapCircleBorderPlayerAngle = 0;

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
        //Rotate_Circle();

        Vector3 reachPosition = target.ClosestPoint(transform.position);
        reachPosition.y = 0;

        transform.position = Vector3.MoveTowards(transform.position, reachPosition, speed * Time.deltaTime);
        RotateToTarget(target.transform.position);

        mapCircleBorderPlayerAngle = -(Quaternion.FromToRotation(Vector3.right, transform.position).eulerAngles.y - 360);
    }

    public void Rotate_Circle()
    {
        //transform.DORotateQuaternion(Quaternion.Euler(0, -mapCircleBorderPlayerAngle, 0), rotationSpeed);
    }

    private Vector3 GetAngleMovement(float angle, float border)
    {
        Vector3 movement = Logic.Trigonometry_XZ(angle * Mathf.Deg2Rad,
                                                 angle * Mathf.Deg2Rad,
                                                 border);

        return movement;
    }

    protected override void MoveStateCheck()
    {
        base.MoveStateCheck();
    }

}
