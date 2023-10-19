using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPositionCreate_Custom : EnemyPositionCreate
{
    public List<Transform> createTransformPositions;

    protected override void SetEnemyCreatePosition()
    {
        foreach (var createTransformPosition in createTransformPositions)
        {
            createPositions.Add(createTransformPosition.position);
        }
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        foreach (var createTransformPosition in createTransformPositions)
        {
            Gizmos.DrawSphere(createTransformPosition.position, gizmoRadius);
        }
    }
}
