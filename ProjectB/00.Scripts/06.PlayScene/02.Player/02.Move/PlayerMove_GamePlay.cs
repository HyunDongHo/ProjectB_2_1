using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState_GamePlay : PlayerMoveState
{
}

public class PlayerMove_GamePlay : PlayerMove
{
    protected float CalculateMoveSpeed(float baseMoveSpeed)
    {
        return baseMoveSpeed * playerControl.GetStats<Stats>().manager.GetValue(PlayerStatsValueDefine.MoveSpeedRatio);
    }
}