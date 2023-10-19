using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Scheduler;
using UnityEngine.AI;

public class EnemyMoveState : MoveState
{
    public const string MOVE_AUTO = "MOVE_AUTO";
    public const string RUN_TO_TARGET = "RUN_TO_TARGET";
}

public class EnemyMove : Move
{
    public EnemyControl enemyControl;

    private TimerBuffer autoChangeDirBuffer = new TimerBuffer(0);
    private TimerBuffer autoChangeWait = new TimerBuffer(0);

    public float minAutoChangeDirTime = 0.0f;
    public float maxAutoChangeDirTime = 0.0f;

    public float minWaitTime = 0.0f;
    public float maxWaitTime = 0.0f;

    [Space]

    private Vector3 originPosition;

    public float moveAvailableRange = 1.5f;

    public float moveSpeed = 0.25f;
    public float runSpeed = 1;
    public float rotationSpeed = 0.1f;
    public float navMaxDistance = 0.1f;

    public Collider moveTarget = null;

    protected Vector3 currentDir = Vector3.zero;

    public void SetOriginPosition(Vector3 position)
    {
        originPosition = position;
    }

    public void DefaultMoveUpdate()
    {
        if (!isAvaliableUpdateMove || isNowNukbackMove) return;

        Move_Auto();
    }

    public void MoveToTargetUpdate(Collider target)
    {
        if (!isAvaliableUpdateMove || isNowNukbackMove) return;

        if (moveTarget == null)
            return;

        enemyControl.GetAttack<Attack>().CompleteAttackWait();

        MoveToTarget(moveTarget);
    }
    public void MoveToTargetUpdate(PlayerControl target)
    {
        if (!isAvaliableUpdateMove || isNowNukbackMove) return;

        enemyControl.GetAttack<Attack>().CompleteAttackWait();

        MoveToTarget(target);
    }

    private void MoveToTarget(Collider target)
    {
        ChangeState(EnemyMoveState.RUN_TO_TARGET);

        if (!isAvailableMove || isNowNukbackMove) return;

        Vector3 dir = (target.transform.position - transform.position).normalized;
        dir.y = 0;
        transform.position += dir * runSpeed * Time.deltaTime;

        RotateToDir(dir);

        enemyControl.PlayRunAnimation();

    }
    private void MoveToTarget(PlayerControl target)
    {
        ChangeState(EnemyMoveState.RUN_TO_TARGET);

        if (!isAvailableMove || isNowNukbackMove) return;

        Vector3 dir = (target.transform.position - transform.position).normalized;
        dir.y = 0;
        transform.position += dir * runSpeed * Time.deltaTime;

        RotateToDir(dir);

        enemyControl.PlayRunAnimation();

    }

    public virtual void Move_Auto()
    {
        ChangeState(EnemyMoveState.MOVE_AUTO);

        if (!isAvailableMove || isNowNukbackMove) return;

      //  if (SceneSettingManager.instance.IsDefaultStageTypeInside())
      //  {
      //      if (!TargetNavigationPosition(transform.position))
      //          currentDir = (originPosition - transform.position).normalized;
      //  }       
        
        if (currentDir != Vector3.zero)
        {
            currentDir.y = 0;
            transform.position += currentDir * moveSpeed * Time.deltaTime;
            RotateToDir(currentDir);

            enemyControl.PlayRunAnimation();
        }
        else
            enemyControl.PlayIdleAnimation();

        if (!autoChangeDirBuffer.isRunningTimer)
        {
            currentDir = Vector3.zero;
            if (!autoChangeWait.isRunningTimer)
            {
                Timer.instance.TimerStart(autoChangeWait,
                    OnComplete: () =>
                    {
                        Timer.instance.TimerStart(autoChangeDirBuffer);

                        Vector3 randomCircle = UnityEngine.Random.insideUnitCircle;
                        currentDir = new Vector3(randomCircle.x, 0, randomCircle.y);

                        autoChangeWait.time = UnityEngine.Random.Range(minWaitTime, maxWaitTime);
                        autoChangeDirBuffer.time = UnityEngine.Random.Range(minAutoChangeDirTime, maxAutoChangeDirTime);
                    });
            }
        }
    }

    public void ResetMoveTarget()
    {
        if(moveTarget != null)
            moveTarget = null;
    }

    private void RotateToDir(Vector3 dir)
    {
        Vector3 lookTargetEulerAngles = Quaternion.FromToRotation(Vector3.forward, dir).eulerAngles;
        transform.eulerAngles = new Vector3(0, lookTargetEulerAngles.y, 0);
    }

    public void SetMonsterTarget(Collider collider)
    {
        moveTarget = collider;
    }

    bool TargetNavigationPosition(Vector3 _pos)
    {
        _pos = _pos + new Vector3(0f, 0, 0f);

        NavMeshHit hit;

        if (NavMesh.SamplePosition(_pos, out hit, navMaxDistance, NavMesh.AllAreas))
        {
            return true;
        }

        return false;
    }
}
