using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAvailableCreate : MonoBehaviour
{
    public virtual bool IsAvailableCreateEnemy(Vector3 createPosition)
    {
        return true;
    }

    public virtual void UsedCreatePosition(Vector3 usedCreatePosition)
    {

    }

    public virtual void NullCreatePosition(List<Vector3> allCreatePositions, int currentEnemyCount)
    {

    }
}
