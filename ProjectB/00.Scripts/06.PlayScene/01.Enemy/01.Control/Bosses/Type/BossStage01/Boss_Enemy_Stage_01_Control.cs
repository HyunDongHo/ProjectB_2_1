using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Enemy_Stage_01_Control : BossEnemyControl
{
    public override void StartIntro(Action OnCompleteBossIntroStart = null)
    {
        base.StartIntro(OnCompleteBossIntroStart);

        subCamera.PlayCameraAnimation("Boss_ST01_Appear_01_Cam_ref", DefineManager.DEFAULT_CAMERA_TIME);
        //GetModel<EnemyModel>().animationControl.PlayAnimation("Boss_ST01_Appear_01",
        //    OnAnimationEnd: () =>
        //    {
        //        OnCompleteBossIntroStart?.Invoke();
        //    });
        GetModel<EnemyModel>().animationControl.PlayAnimation("Boss_ST01_Idle01",
            OnAnimationEnd: () =>
            {
                OnCompleteBossIntroStart?.Invoke();
            });
        Debug.Log("StartIntro");
    }

    public override void EndIntro(Action OnCompleteBossIntroEnd = null)
    {
        base.EndIntro();

        GetModel<EnemyModel>().animationControl.PlayAnimation("Boss_ST01_Idle01", isRepeat: true, OnAnimationEnd: () => OnCompleteBossIntroEnd?.Invoke());
    }

    protected override string GetIdle()
    {
        return "Boss_ST01_Idle01";
    }

    protected override string GetDie()
    {
        return "Boss_ST01_Die";
    }

    public override string GetHit()
    {
        return "Boss_ST01_Damage";
    }
    protected override string GetRun()
    {
        return "Boss_ST01_Run";
    }
}
