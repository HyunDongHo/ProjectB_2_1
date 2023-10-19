using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;

public class EnemyAvailableCreate_InsideStage : EnemyAvailableCreate
{
    public Transform target;
    public int notCreateRange = 10;

    public float minReAvailableCreateTime = 30;
    public float maxReAvailableCreateTime = 60;

    private List<Vector3> createdPositions = new List<Vector3>();
    private Dictionary<Vector3, TimerBuffer> removeCreatedPositions = new Dictionary<Vector3, TimerBuffer>();

    public override bool IsAvailableCreateEnemy(Vector3 createPosition)
    {
        bool isAvailableCreate = !createdPositions.Exists(data => data == createPosition) && Vector3.Distance(target.transform.position, createPosition) > notCreateRange;

        return isAvailableCreate;
    }

    public override void UsedCreatePosition(Vector3 usedCreatePosition)
    {
        createdPositions.Add(usedCreatePosition);

        if(!removeCreatedPositions.ContainsKey(usedCreatePosition))
            removeCreatedPositions.Add(usedCreatePosition, new TimerBuffer(Random.Range(minReAvailableCreateTime, maxReAvailableCreateTime)));

        Timer.instance.TimerStart(removeCreatedPositions[usedCreatePosition], 
            OnComplete : () =>
            {
                removeCreatedPositions.Remove(usedCreatePosition);
                createdPositions.Remove(usedCreatePosition);
            });
    }

    public override void NullCreatePosition(List<Vector3> allCreatePositions, int currentEnemyCount)
    {
        if(currentEnemyCount <= 0)
        {
            List<Vector3> availableCreatePositions = new List<Vector3>(allCreatePositions);

            for (int i = 0; i < createdPositions.Count; i++)
                availableCreatePositions.Remove(createdPositions[i]);

            if (availableCreatePositions.Count <= 0)
            {
                List<Vector3> convertAllCreatePositions = allCreatePositions.FindAll(data => Vector3.Distance(target.transform.position, data) > notCreateRange);
                Vector3 removeCreatedPosition = convertAllCreatePositions[Random.Range(0, convertAllCreatePositions.Count)];

                Timer.instance.TimerStop(removeCreatedPositions[removeCreatedPosition]);

                removeCreatedPositions.Remove(removeCreatedPosition);
                createdPositions.Remove(removeCreatedPosition);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target.transform.position, notCreateRange);
    }
}
