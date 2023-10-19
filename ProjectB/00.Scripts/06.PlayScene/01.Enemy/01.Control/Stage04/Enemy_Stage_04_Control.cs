using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Stage_04_Control : EnemyControl
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
        return "Enemy_ST04_001_Idle_01";
    }

    protected override string GetDie()
    {
        return "Enemy_ST04_001_Death";
    }

    public override string GetHit()
    {
        return "Enemy_ST04_001_Hit_Front";
    }

    protected override string GetWalk()
    {
        return "Enemy_ST04_Walk";
    }

    protected override string GetRun()
    {
        return "Enemy_ST04_Run";
    }
}
