using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using Scheduler;

public class PlayerMove_InSideStage : PlayerMove_DefaultStage
{
    public NavMeshAgent agent;

    protected override void MoveDefault(float speed)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, Mathf.Infinity, ~LayerMask.GetMask("Player") & ~LayerMask.GetMask("Ignore Raycast"));

        if(hits.Length > 0)
        {
            Collider nearestObject = hits[Logic.GetNearestObjectIndex(transform.position, hits, "Enemy")];

            Vector3 reachPosition = nearestObject.ClosestPoint(transform.position);
            reachPosition.y = 0;

            agent.updateRotation = true;

            agent.speed = speed;
            agent.acceleration = float.MaxValue;

            agent.SetDestination(reachPosition);
        }
    }

    protected override void MoveToTarget(float speed, Collider target)
    {
        Vector3 reachPosition = target.ClosestPoint(transform.position);
        reachPosition.y = 0;

        agent.updateRotation = false;
        RotateToTarget(target.transform.position);

        agent.speed = speed;
        agent.acceleration = float.MaxValue;

        agent.SetDestination(reachPosition);
    }

    private void RotateToTarget(Vector3 reachPosition)
    {
        if (reachPosition - transform.position == Vector3.zero) return;

        Vector3 lookTargetEulerAngles = Quaternion.FromToRotation(Vector3.forward, (reachPosition - transform.position).normalized).eulerAngles;
        playerControl.transform.eulerAngles = new Vector3(0, lookTargetEulerAngles.y, 0);
    }

    protected override void MoveStateCheck()
    {
        base.MoveStateCheck();

        agent.isStopped = !isAvailableMove;
    }
}