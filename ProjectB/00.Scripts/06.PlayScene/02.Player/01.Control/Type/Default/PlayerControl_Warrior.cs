using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Scheduler;

public class PlayerControl_Warrior : PlayerControl_GamePlay
{
    private Dictionary<ScreenRectType, bool> dashAttackPreAttackCheck = new Dictionary<ScreenRectType, bool>() { { ScreenRectType.Left, false }, { ScreenRectType.Right, false } };

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (!isEnabledControl || !isAvailableControl || isEndGamePlay || !GetAttack<Attack>().isEnableAttack) return;

        if (!GetStats<PlayerStats>().hp.isAlive) return;

        GetAttack<PlayerAttack>().ResetAttackTargets();
      //  GetAttack<PlayerAttack>().ResetFrontAttackTargets();

        (pRaycast as PlayerRaycast_DefaultStage).UpdateRaycast();   

        base.Update();
    }

}
