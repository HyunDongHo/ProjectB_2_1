using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast_Warrior : PlayerRaycast_DefaultStage
{
    public override void UpdateRaycast()
    {
        // if (dashAttackCheckRaycast.GetMaxColliders().Length > 0 && dashAttackCheckRaycast.GetMinColliders().Length <= 0)
        // {
        //   Collider[] dashAttackCheck = dashAttackCheckRaycast.GetRaycastHit();
        //    OnDashAttackCheckHit?.Invoke(dashAttackCheck);
        // }

        Collider[] attackRange = attackRangeRaycast.GetRaycastHit();
        if (attackRange.Length > 0)
        {
            OnAttackRangeHit?.Invoke(attackRange);
            OnAttackHit?.Invoke(attackRange);
        }
        else
        {            
            OnDashToTarget?.Invoke(dashToTargetRaycast.GetRaycastHit());
        }
    }
}
