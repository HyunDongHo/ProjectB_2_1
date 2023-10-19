using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCheck_Capsule : RaycastCheck
{
    public float height;
    public float radius;

    protected override Collider[] CheckPhysicsOverlap(Vector3 pos)
    {
        return Physics.OverlapCapsule(pos + Vector3.up * height / 2,
                                      pos - Vector3.up * height / 2,
                                      radius, layerMask);
    }

    protected override void DrawRaycast(Vector3 pos)
    {
        GizmoExtension.DrawWireCapsule(pos, radius, height, Color.red);
    }

    protected override Vector3 ConvertOffset(Vector3 offset)
    {
        Vector3 convertOffset = Vector3.zero;

        convertOffset = Vector3.right * offset.x + Vector3.up * offset.y + target.forward * offset.z;

        return convertOffset;
    }
}
