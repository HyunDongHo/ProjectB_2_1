using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Scheduler;
using UnityEngine.AI;

public class BossEnemyMove : EnemyMove
{
    public PlayerControl targetControl;

    public override void Move_Auto()
    {
        ChangeState(EnemyMoveState.MOVE_AUTO);

        if (!isAvailableMove || isNowNukbackMove) return;


    }

}
