using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyAttack : EnemyAttack
{
    protected const string BOUND_PLAYER = "BOUND_PLAYER";
    protected const string KNOCKBACK_PLAYER = "KNOCKBACK_PLAYER";
    protected const string RESET_TARGET = "RESET_TARGET";

    //protected override void AddAttackDataEvent(AttackData attackData)
    //{
    //    attackData.AddHandleEvent(CommonEnemyAttackDataParameterDefine.ATTACK,
    //        (parameter) =>
    //        {
    //            CheckPlayerCurrentMoveIndexAndAttack(attackData, parameter.intValue);
    //        });
    //}
    protected void AddAttackControlDataEvent(AttackData attackData)
    {
        attackData.AddHandleEvent(CommonEnemyAttackDataParameterDefine.BOUND,
            (parameter) =>
            {
                foreach (var target in attackTargets.Values)
                {
                    if ((target as PlayerControl_DefaultStage)?.GetMove<Move>().isNowBound == false)
                    {
                        (target as PlayerControl_DefaultStage)?.PlayBoundAnimation(null);
                        (target as PlayerControl_DefaultStage)?.GetMove<Move>().Bound(parameter.floatValue, parameter.floatValue1, parameter.floatValue2, parameter.floatValue3, -target.transform.forward);
                    }
                   // else
                   // {
                   //     (target as PlayerControl_DefaultStage)?.GetMove<Move>().KnockBack(parameter.floatValue1, parameter.floatValue2, -target.transform.forward);
                   // }
                }
            });

        attackData.AddHandleEvent(CommonEnemyAttackDataParameterDefine.KNOCK_DOWN,
         (parameter) =>
         {
             foreach (var target in attackTargets.Values)
             {

                 (target as PlayerControl_DefaultStage)?.GetMove<Move>().KnockBack(parameter.floatValue, parameter.floatValue1, -target.transform.forward);
             }
         });

        attackData.AddHandleEvent(CommonEnemyAttackDataParameterDefine.PLUCK,
         (parameter) =>
         {
             foreach (var target in attackTargets.Values)
             {
                 Vector3 dir = transform.position - target.transform.position;
                 (target as PlayerControl_DefaultStage)?.GetMove<Move>().Pluck(dir.magnitude * 0.3f, 0.5f, dir);
             }
         });

        attackData.AddHandleEvent(CommonEnemyAttackDataParameterDefine.RESET_ATTACK_RAYCAST,
       (parameter) =>
       {
           (control as BossEnemyControl)?.eRaycast.SetRaycast(parameter.intValue);
       });

     attackData.AddHandleEvent(CommonEnemyAttackDataParameterDefine.RESET_TARGET,
         (parameter) =>
          {
              (control as EnemyControl)?.GetMove<EnemyMove>().SetMonsterTarget(null);
         });
    }

    private void CheckPlayerCurrentMoveIndexAndAttack(AttackData attackData, int checkIndex)
    {
        int moveAreaIndex = StageManager.instance.playerControl.GetMove<PlayerMove_BossStage>().GetCurrentMoveAreaIndex();

        if (moveAreaIndex == checkIndex)
        {
            int totalCount = GetAttackDataEventCount(attackData);  
            AttackTargets(totalCount);
        }
    }
}
