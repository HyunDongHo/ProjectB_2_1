using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAvailableCreate_BossStage : EnemyAvailableCreate
{
    public override bool IsAvailableCreateEnemy(Vector3 createPosition)
    {
        return true;
    }
}
