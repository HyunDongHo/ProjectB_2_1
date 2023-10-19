using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Scheduler;
using RNG;

public class EnemyControl : Control, ObjectPoolInterface
{
    public EnemyType enemyType = EnemyType.Normal;

    public EnemyRaycast eRaycast;
    public EnemyReward enemyReward;

    private const int DEFAULT_HIT_MOTION_SKIP_START_FRAME = 3;

    public float fallDownDistance = 10.0f;
    public float fallDownUpPower = 1.5f;
    public float fallDownEndNormalized = 0.65f;

    public Action<EnemyControl> OnHpExhausted;

    protected bool isPlayHitMotion = false;

    protected override void Awake()
    {
        base.Awake();
        eRaycast.enemyControl = this;
        Respawned();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected override void Update()
    {
        if (!isEnabledControl || !isAvailableControl) return;

        if (GetMove<Move>().isNowNukbackMove || GetMove<Move>().isNowBound) return;

        if (!GetStats<EnemyStats>().hp.isAlive) return;

        GetAttack<EnemyAttack>().ResetAttackTargets();
        eRaycast.UpdateRaycast();
    }

    protected override void AddEvent()
    {
        GetStats<EnemyStats>().hp.OnHpReduce += HandleOnHpReduce;

        eRaycast.OnAttackHit += HandleOnAttackHit;
        eRaycast.OnDetect += HandleOnDetect;
    }

    protected override void RemoveEvent()
    {
        GetStats<EnemyStats>().hp.OnHpReduce -= HandleOnHpReduce;

        eRaycast.OnAttackHit -= HandleOnAttackHit;
        eRaycast.OnDetect -= HandleOnDetect;
    }

    #region 탐지하여 작동하는 기능

    protected virtual void HandleOnAttackHit(Collider[] cols)
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

    //protected virtual void HandleOnDetect(Collider[] cols)
    //{
    //    if (isPlayHitMotion) return;

    //    EnemyMove move = GetMove<EnemyMove>();

    //    if (cols.Length > 0)
    //    {
    //        if(move.moveTarget == null)
    //        {
    //            int randNum = UnityEngine.Random.Range(0, cols.Length);
    //            move.moveTarget = cols[randNum];
    //        }            
             
    //        move.MoveToTargetUpdate(move.moveTarget);
    //    }
    //    else
    //    {
    //        move.moveTarget = null;
    //        return;
    //        //move.DefaultMoveUpdate();  
    //    }
    //}
    protected virtual void HandleOnDetect(Collider[] cols)
    {
        if (isPlayHitMotion) return;

        EnemyMove move = GetMove<EnemyMove>();

        if (cols.Length > 0)
        {
            PlayerControl playerControl = PlayersControlManager.instance.GetNearActivePlayer(transform.position);
            
            if (playerControl == null)
            {
                move.moveTarget = null;
                return;
            }

            for (int i=0; i < cols.Length; ++i)
            {
                if (playerControl.gameObject.name == cols[i].gameObject.name)
                {
                    move.moveTarget = cols[i];
                    move.MoveToTargetUpdate(move.moveTarget);
                    return;
                }
            }

            move.moveTarget = null;
            return;

          // PlayerControl playerControl = PlayersControlManager.instance.GetNearActivePlayer(transform.position);
          //
          // if (playerControl == null)
          // {
          //     move.moveTarget = null;
          //     return;
          // }
          //
          // if (playerControl.gameObject.name == move.moveTarget.name)
          // {
          //     move.moveTarget = cols
          //     move.MoveToTargetUpdate(move.moveTarget);
          // }
          //
          //
          // if (move.moveTarget == null)
          // {
          //     int randNum = UnityEngine.Random.Range(0, cols.Length);
          //     move.moveTarget = cols[randNum];
          // }
          // else
          // {
          //     PlayerControl playerControl = PlayersControlManager.instance.GetNearActivePlayer(transform.position);
          //
          //     if (playerControl == null)
          //     {
          //         move.moveTarget = null;
          //         return;
          //     }
          //
          //     if(playerControl.gameObject.name == move.moveTarget.name)
          //     {
          //         move.MoveToTargetUpdate(move.moveTarget);
          //     }
          //    // else
          //    // {
          //    //     move.moveTarget = null;  
          //    //     return;
          //    // }
          // }

        }
        else
        {
            move.moveTarget = null;
            return;
            //move.DefaultMoveUpdate();  
        }
    }

    #endregion

    #region HP 컨트롤 관련

    private void HandleOnHpReduce(float hp)
    {
        if (hp <= 0)
            HpExhausted();
    }

    protected virtual void HpExhausted()
    {
        ResetState(false);

        if (GetStats<Stats>().hp.lastAffectObject != null)
        {
            GameObject killedObj = GetStats<Stats>().hp.lastAffectObject;
            StageManager.instance.canvasManager.GetUIManager<UIManager_Stage>().playerUI.StartMonsterKillAnim(killedObj.name);
        }

        OnHpExhausted?.Invoke(this);

        PlayDieAnimation(OnEnd: () => ObjectPoolManager.instance.RemoveObject(gameObject));
        transform.DOJump(transform.position - transform.forward * fallDownDistance, fallDownUpPower, numJumps: 1, GetModel<EnemyModel>().animationControl.GetTotalTime(GetDie()) * fallDownEndNormalized);
    }

    public void Respawned()
    {
        ResetState(true);

        //GetStats<EnemyStats>().hp.SetHpToMax();
        GetStats<EnemyStats>().hp.SetHpToMax(enemyType);  
        //Debug.Log($"{this.gameObject.name} hp : { GetStats<EnemyStats>().hp.GetCurrentHp()}");    

        GetMove<EnemyMove>().SetOriginPosition(transform.position);
    }

    private void ResetState(bool active)
    {
        isEnabledControl = active;

        GetComponent<CapsuleCollider>().enabled = active;

        GetMove<EnemyMove>().isAvailableMove = active;

        GetAttack<EnemyAttack>().StopAttack();
        GetAttack<EnemyAttack>().CompleteAttackWait();

        GetMove<EnemyMove>().ResetMove();

        ShaderFunction.instance.AllReset(GetModel<EnemyModel>());
    }

    #endregion

    #region 피격 

    public void GotBound(Vector3 dir)
    {
        ResetState(false);

        PlayDieAnimation(OnEnd: () => ObjectPoolManager.instance.RemoveObject(gameObject));
        transform.DOJump(transform.position - transform.forward * fallDownDistance, fallDownUpPower, numJumps: 1, GetModel<EnemyModel>().animationControl.GetTotalTime(GetDie()) * fallDownEndNormalized);
    }

    protected override void GotAttacked(bool isPlayHitMotion, int hitMotionSkipStartFrame)
    {
        if (!isEnabledControl || !isAvailableControl) return;

        ShaderFunction.instance.Play_Hit(GetModel<EnemyModel>(), 0.4f);

        if (isPlayHitMotion)
            GotHitted(hitMotionSkipStartFrame);
    }

    private void GotHitted(int hitSkip)
    {
        StartHitMotion();

        EnemyMove move = GetMove<EnemyMove>();
        EnemyModel model = GetModel<EnemyModel>();

        float hitTotalTime = model.animationControl.GetTotalTime(GetHit());
        float hitTotalFrame = model.animationControl.GetTotalFrame(GetHit());
        float oneFrameTime = hitTotalTime / hitTotalFrame;

       // move.KnockBack(2, hitTotalTime);

        TimerBuffer hitBuffer = new TimerBuffer(hitTotalTime);
        TimerFunctions.instance.TimerFrameToFixedOneTime(hitBuffer, oneFrameTime,
            OnFrame: (frame) =>
            {
                if (hitSkip == -1 || !move.CheckState(MoveState.STOP)) return;

                UpdateHitMotion(hitBuffer, frame, hitSkip);
            },
            OnComplete: () => EndHitMotion(hitBuffer));
    }

    private void StartHitMotion()
    {
        PlayHitAnimation(OnEnd: () => PlayIdleAnimation());

        isPlayHitMotion = true;

        EnemyAttack attack = GetAttack<EnemyAttack>();
        attack.StopAttack();
    }

    private void UpdateHitMotion(TimerBuffer timerBuffer, int frame, int hitMotionSkipStartFrame)
    {
        EnemyAttack attack = GetAttack<EnemyAttack>();

        if (frame >= hitMotionSkipStartFrame && attack.IsAvaliableAttack())
            EndHitMotion(timerBuffer);
    }

    private void EndHitMotion(TimerBuffer timerBuffer)
    {
        isPlayHitMotion = false;

        if (!GetStats<Stats>().hp.isAlive) return;

        Timer.instance.TimerStop(timerBuffer);
    }

    #endregion

    #region 값 계산

    protected override float CalculateHitPercentage(float attackerDamage, float defenderDefense, int attackerLevel, int defenderLevel)
    {
        // 적에게는 명중률 세팅을 하지 않기 때문에 무조건 True가 되도록 100의 확률을 보내줌.
        return 100;
    }

    protected override float CalculateEvasionPercentage(int attackerLevel, int defenderLevel)
    {
        float baseValue = base.CalculateEvasionPercentage(attackerLevel, defenderLevel);

        return baseValue;
    }

    protected override float CalculateDamageReduceRatio(int divideDamage, float attackerDamage, float defenderDefense, int attackerLevel, int defenderLevel)
    {
        float baseValue = base.CalculateDamageReduceRatio(divideDamage, attackerDamage, defenderDefense, attackerLevel, defenderLevel);

        return baseValue;
    }

    #endregion

    #region Animation

    public void PlayIdleAnimation(bool isRepeat = true)
    {
        GetAttack<EnemyAttack>().shootDone = true;  
        GetModel<Model>().animationControl.PlayAnimation(GetIdle(), isSameAniAvailablePlay: false, isRepeat: isRepeat);
    }

    public void PlayDieAnimation(Action OnEnd)
    {
        GetModel<Model>().animationControl.PlayAnimation(GetDie(), weight: (int)AnimationWeight.Enemy.HpExhausted, OnAnimationEnd: OnEnd);
    }

    public void PlayHitAnimation(Action OnEnd)
    {
        GetModel<Model>().animationControl.PlayAnimation(GetHit(), isSameAniAvailablePlay: false, weight: (int)AnimationWeight.Enemy.HpReduce, OnAnimationEnd: OnEnd);
    }
    public void PlayBoundAnimation(Action OnEnd)
    {
        if ((this as BossEnemyControl) != null)
            return;

        GetModel<Model>().animationControl.PlayAnimation(GetBound(),isSameAniAvailablePlay:false, weight: (int)AnimationWeight.Enemy.Bound, OnAnimationEnd: OnEnd);  
    }

    public void PlayRunAnimation(bool isRepeat = true)
    {
        GetAttack<EnemyAttack>().shootDone = true;
        GetModel<Model>().animationControl.PlayAnimation(GetRun(), isSameAniAvailablePlay: false, isRepeat: isRepeat);
        //Debug.Log($"GetRun() : {GetRun()}");  
    }

    protected virtual string GetIdle()
    {
        return string.Empty;
    }

    protected virtual string GetDie()
    {
        return string.Empty;
    }

    public virtual string GetHit()
    {
        return string.Empty;
    }
    protected virtual string GetWalk()
    {
        return string.Empty;
    }

    protected virtual string GetRun()
    {
        return string.Empty;
    }
    protected virtual string GetBound()
    {
        return string.Empty;
    }
    #endregion

    protected override int GetInitHitMotionSkipStartFrame()
    {
        return DEFAULT_HIT_MOTION_SKIP_START_FRAME;
    }
}
