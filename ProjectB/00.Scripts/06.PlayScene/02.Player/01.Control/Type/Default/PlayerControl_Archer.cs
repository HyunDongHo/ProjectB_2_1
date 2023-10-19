using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Scheduler;

public class PlayerControl_Archer : PlayerControl_DefaultStage
{
    protected override void Start()
    {
        base.Start();

    }

    //protected override void AddEvent()
    //{
    //    PlayerRaycast_DefaultStage raycast = pRaycast as PlayerRaycast_DefaultStage;
    //    raycast.OnAttackRangeHit += HandleOnAttackRangeHit;
    //    raycast.OnAttackHit += HandleOnAttackHit;
    //    raycast.OnDashToTarget += HandleOnDashToTarget;
    //}

    //protected override void RemoveEvent()
    //{
    //    PlayerRaycast_DefaultStage raycast = pRaycast as PlayerRaycast_DefaultStage;
    //    raycast.OnAttackRangeHit -= HandleOnAttackRangeHit;
    //    raycast.OnAttackHit -= HandleOnAttackHit;
    //    raycast.OnDashToTarget -= HandleOnDashToTarget;
    //}

    protected override bool CheckAvailableUseSkill(int skillIndex)
    {
        PlayerRaycast_DefaultStage raycast = pRaycast as PlayerRaycast_DefaultStage;

        bool available = true;

        // 스킬 1번과 2번은 근접이기 때문에 Attack에 들어오지 않으면 사용 불가.
        Collider[] attack = raycast.attackRaycast.GetRaycastHit();
        if ((skillIndex == 0 || skillIndex == 1 || skillIndex == 2 || skillIndex == 3) && attack.Length <= 0)            
            available = false;

       // Collider[] attackRange = raycast.attackRangeRaycast.GetRaycastHit();
       // if (skillIndex == 3 && attackRange.Length <= 0)
       //     available = false;

        return available;
    }



}
