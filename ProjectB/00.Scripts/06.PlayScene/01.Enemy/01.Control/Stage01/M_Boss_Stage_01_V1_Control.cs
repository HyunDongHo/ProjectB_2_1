using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Boss_Stage_01_V1_Control : EnemyControl
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
        return "M_Boss_ST01_Idle01";
    }

    protected override string GetDie()
    {
        return "M_Boss_ST01_Die";
    }

    public override string GetHit()
    {
        return "M_Boss_ST01_Damage";
    }

    protected override string GetWalk()
    {
        return "M_Boss_ST01_Run";
    }

    protected override string GetRun()
    {
        return "M_Boss_ST01_Run";
    }
    protected override string GetBound()
    {
        return "M_Boss_ST01_Bound";  
    }
}
