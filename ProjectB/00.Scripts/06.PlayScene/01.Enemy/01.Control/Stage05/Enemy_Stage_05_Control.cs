using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Stage_05_Control : EnemyControl
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
        return "M_Boss_ST01_Idle";
    }

    protected override string GetDie()
    {
        return "M_Boss_ST01_Death";
    }

    public override string GetHit()
    {
        return "M_Boss_ST01_Hit_Front";
    }

    protected override string GetWalk()
    {
        return "M_Boss_ST01_Walk";
    }

    protected override string GetRun()
    {
        return "M_Boss_ST01_Run";
    }
}
