using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public abstract class RaycastCheck : MonoBehaviour
{
    public Action<Collider[]> OnHit;

    public bool isFollowAnotherTransform = false;
    [ConditionalHide(nameof(isFollowAnotherTransform), true)] public Transform target;

    public Vector3 offset = Vector3.zero;

    protected LayerMask layerMask;

    public void SetUp(LayerMask layerMask)
    {
        this.layerMask = layerMask;
    }

    private void Start()
    {
        if (!isFollowAnotherTransform)
            target = transform;
    }

    protected Vector3 GetRaycastPos()
    {
        return transform.position + ConvertOffset(offset);
    }

    public void UpdateRaycastHit()
    {
        Collider[] cols = CheckPhysicsOverlap(GetRaycastPos());

        if (cols.Length > 0)
            OnHit?.Invoke(cols);
    }

    public Collider[] GetRaycastHit()
    {
        Collider[] cols = CheckPhysicsOverlap(GetRaycastPos());

        return cols;
    }

    private void OnDrawGizmosSelected()
    {
        DrawRaycast(GetRaycastPos());
    }

    protected abstract Collider[] CheckPhysicsOverlap(Vector3 pos);
    protected abstract void DrawRaycast(Vector3 pos);
    protected abstract Vector3 ConvertOffset(Vector3 offset);
}
