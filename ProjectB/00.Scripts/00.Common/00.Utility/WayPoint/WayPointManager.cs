using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointManager : MonoBehaviour
{
    public Action OnWayPointEnd;

    private WayPoint currentWayPoint;

    public void Init(WayPointParent wayPointParent)
    {
        currentWayPoint = wayPointParent.startWayPoint.GetNextWayPoint();
    }

    public bool IsCurrenteWayPointDivideBranch()
    {
        return currentWayPoint.IsCurrentWayPointDivideBranch();
    }

    public WayPoint GetCurrentWayPoint()
    {
        return currentWayPoint;
    }

    public WayPoint GetNextWayPoint(int branchIndex = 0)
    {
        return currentWayPoint?.GetNextWayPoint(branchIndex);
    }

    public void SetNextWayPoint(int branchIndex = 0)
    {
        if (currentWayPoint == null || currentWayPoint is EndWayPoint endWayPoint)
        {
            OnWayPointEnd?.Invoke();
        }

        currentWayPoint = currentWayPoint?.GetNextWayPoint(branchIndex);
    }
}
