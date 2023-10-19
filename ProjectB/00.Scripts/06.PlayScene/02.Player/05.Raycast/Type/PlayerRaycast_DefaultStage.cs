using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast_DefaultStage : PlayerRaycast_GamePlay
{
    public Action<Collider[]> OnDashToTarget;
    public RaycastCheck_Capsule dashToTargetRaycast;

    //public Action<Collider[]> OnDashAttackCheckHit;
  //  public RaycastCheck_Capsule_MinMax dashAttackCheckRaycast;

    protected override void Awake()
    {
        base.Awake();

        LayerMask layerMask = ~LayerMask.GetMask("Player") & ~LayerMask.GetMask("Ignore Raycast") & ~LayerMask.GetMask("Default");

        dashToTargetRaycast.SetUp(layerMask);
        targetCollider = null;
      //  dashAttackCheckRaycast.SetUp(layerMask);
    }

    public virtual void UpdateRaycast()
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
        }

        Collider[] attack = attackRaycast.GetRaycastHit();

        if (attack.Length > 0)
        {            
            OnAttackHit?.Invoke(attack);
        }
        else
        {            
            OnDashToTarget?.Invoke(dashToTargetRaycast.GetRaycastHit());
        }
    }
}
