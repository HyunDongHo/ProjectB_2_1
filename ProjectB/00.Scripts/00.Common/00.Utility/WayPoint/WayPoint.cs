using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public List<WayPoint> nextWayPoint = new List<WayPoint>();

    public bool IsCurrentWayPointDivideBranch()
    {
        return nextWayPoint.Count > 1;
    }

    public WayPoint GetNextWayPoint(int branchIndex = 0)
    {
        if (nextWayPoint.Count <= 0)
            return null;

        return nextWayPoint[branchIndex];
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawSphere(GetPosition(), 0.25f);

        Gizmos.color = Color.green;

        foreach (WayPoint wayPoint in nextWayPoint)
        {
            Gizmos.DrawLine(GetPosition(), wayPoint.GetPosition());
        }
    }
}
