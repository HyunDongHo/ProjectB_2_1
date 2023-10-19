using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast_GamePlay : MonoBehaviour
{
    public Action<Collider[]> OnAttackHit;
    public RaycastCheck_Capsule attackRaycast;

    public Action<Collider[]> OnAttackRangeHit;
    public RaycastCheck_Capsule attackRangeRaycast;

    public Collider targetCollider = null;

    protected virtual void Awake()
    {
        LayerMask layerMask = ~LayerMask.GetMask("Player") & ~LayerMask.GetMask("Ignore Raycast");

        attackRaycast.SetUp(layerMask);
        attackRangeRaycast.SetUp(layerMask);
    }
}
