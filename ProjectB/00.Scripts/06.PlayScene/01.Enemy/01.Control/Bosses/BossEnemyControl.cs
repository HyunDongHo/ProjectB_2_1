using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyControl : EnemyControl
{
    public SubCamera subCamera;



    protected override void Awake()
    {
        base.Awake();

        //PlayersControl playersControl = (StageManager.instance as OutsideStageManager)?.playersControl;
        //for (int i = 0; i < playersControl.playersContol.Length; ++i)
        //{
        //    playersControl.playersContol[i].OnHpExhaustedImmediately += HandleOnPlayerDie;
        //}
        PlayerControl[] playersControl = PlayersControlManager.instance.playersContol;
        for (int i = 0; i < playersControl.Length; ++i)
        {
            playersControl[i].OnHpExhaustedImmediately += HandleOnPlayerDie;
        }
    }

    private  void HandleOnPlayerDie(PlayerControl playerControl)
    {
        GetMove<EnemyMove>().moveTarget = null;
    }

    public virtual void StartIntro(Action OnCompleteBossIntroStart = null)
    {
        GetModel<Model>().animationControl.ResetAnimationState();
    }

    public virtual void EndIntro(Action OnCompleteBossIntroEnd = null)
    {
        subCamera.StopPlayCameraAnimation();

        GetModel<Model>().animationControl.ResetAnimationState();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        //PlayersControl playersControl = (StageManager.instance as OutsideStageManager)?.playersControl;
        //for (int i = 0; i < playersControl.playersContol.Length; ++i)
        //{
        //    playersControl.playersContol[i].OnHpExhaustedImmediately -= HandleOnPlayerDie;
        //}
        PlayerControl[] playersControl = PlayersControlManager.instance.playersContol;
        for (int i = 0; i < playersControl.Length; ++i)
        {
            playersControl[i].OnHpExhaustedImmediately -= HandleOnPlayerDie;
        }
    }

    protected override void HandleOnAttackHit(Collider[] cols)
    {
        EnemyMove move = GetMove<EnemyMove>();
        EnemyAttack attack = GetAttack<EnemyAttack>();

        if (!move.CheckState(MoveState.STOP))
        {
            move.ResetMove();
        }

        attack.AddAttackTargets(cols);
        attack.StartRandomAttack();
    }

    //protected override void HandleOnDetect(Collider[] cols)
    //{
    //}

    //protected override void HpExhausted()
    //{
    //    GetAttack<EnemyAttack>().StopAttack();

    //    OnHpExhausted?.Invoke(this);
    //}

    public virtual void EndBoss()
    {
        base.HpExhausted();      
    }
}
