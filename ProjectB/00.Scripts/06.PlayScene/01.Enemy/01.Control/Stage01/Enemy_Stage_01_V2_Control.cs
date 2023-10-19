using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Stage_01_V2_Control : EnemyControl
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected override string GetIdle()
    {
        return "Enemy_ST01_002_Idle"; 
    }

    protected override string GetDie()
    {
        return "Enemy_ST01_002_Die";
    }

    public override string GetHit()
    {
        return "Enemy_ST01_002_Damage";    
    }

    protected override string GetWalk()
    {
        return "Enemy_ST01_002_Run";
    }

    protected override string GetRun()
    {
        return "Enemy_ST01_002_Run";
    }
    protected override string GetBound()
    {
        return "Enemy_ST01_002_Bound";
    }
}
