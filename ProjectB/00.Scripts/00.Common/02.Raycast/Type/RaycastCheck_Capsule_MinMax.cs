using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCheck_Capsule_MinMax : RaycastCheck
{
    public float minHeight;
    public float minRadius;

    public float maxHeight;
    public float maxRadius;

    public Collider[] GetMaxColliders()
    {
        return Physics.OverlapCapsule(GetRaycastPos() + Vector3.up * maxHeight / 2,
                                      GetRaycastPos() - Vector3.up * maxHeight / 2,
                                      maxRadius, layerMask);
    }

    public Collider[] GetMinColliders()
    {
        return Physics.OverlapCapsule(GetRaycastPos() + Vector3.up * minHeight / 2,
                                      GetRaycastPos() - Vector3.up * minHeight / 2,
                                      minRadius, layerMask);
    }

    protected override Collider[] CheckPhysicsOverlap(Vector3 pos)
    {
        List<Collider> colliders = new List<Collider>();

        Collider[] maxColliders = GetMaxColliders();
        Collider[] minColliders = GetMinColliders();

        colliders.AddRange(maxColliders);
        foreach (var collider in minColliders)
            colliders.Remove(collider);

        return colliders.ToArray();
    }

    protected override void DrawRaycast(Vector3 pos)
    {
        GizmoExtension.DrawWireCapsule(pos, minRadius, minHeight, Color.green);
        GizmoExtension.DrawWireCapsule(pos, maxRadius, maxHeight, Color.green);
    }

    protected override Vector3 ConvertOffset(Vector3 offset)
    {
        Vector3 convertOffset = Vector3.zero;

        convertOffset = Vector3.right * offset.x + Vector3.up * offset.y + target.forward * offset.z;

        return convertOffset;
    }
}
