using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;
using RNG;
using CodeStage.AntiCheat.ObscuredTypes;

public class Attack : MonoBehaviour
{
    public Control control;

    [Space]

    public bool isEnableAttack = true;

    protected TimerBuffer attackWaitBuffer = new TimerBuffer(0);
    public ObscuredFloat attackWait = 1.0f;

    [System.Serializable]
    public class CurrentAttack
    {
        public TimerBuffer attackBuffer = new TimerBuffer(0);

        public Action currentAttackAction { get; set; } = null;
        public float currentAttackNormalizedTime { get; set; } = 0;

        public bool isAttack { get; set; } = false;

        public void Reset()
        {
            currentAttackAction = null;
            currentAttackNormalizedTime = 0;

            isAttack = false;
        }
    }
    public CurrentAttack currentAttack { get; set; } = new CurrentAttack();

    protected Dictionary<Collider, Control> attackTargets = new Dictionary<Collider, Control>();
    protected Dictionary<Collider, Control> frontTargets = new Dictionary<Collider, Control>();

    protected virtual void Awake()
    {
        InitAttackWait();
    }

    // Attack 후 Delay 설정.
    #region Attack Wait

    private void InitAttackWait()
    {
        SetAttackWaitTime(attackWait);
        CompleteAttackWait();
    }

    public void SetAttackWaitTime(float time)
    {
        attackWait = time;
        attackWaitBuffer.time = time;
    }

    public void CompleteAttackWait()
    {
        attackWaitBuffer.timer = attackWaitBuffer.time;
    }

    public void StartAttackWait(bool isReset = true)
    {
        Timer.instance.TimerStart(attackWaitBuffer, isReset: isReset);
    }

    public void StopAttackWait(bool isReset = true)
    {
        Timer.instance.TimerStop(attackWaitBuffer, isReset: isReset);
    }

    #endregion

    #region Attack Avaliable

    public bool IsAvaliableAttack()
    {
        return isEnableAttack && attackWaitBuffer.isTimerEnd;
    }
    public void RefreshAttackWaitBuffer()
    {
        attackWaitBuffer.timer = attackWaitBuffer.time;
    }

    #endregion

    #region Attack Target Set

    // 공격할 Target를 설정함.
    public void AddAttackTargets(params Collider[] targets)
    {
        foreach (var target in targets)
        {
            // AttackTarget에 이미 Target이 포함되어 있으면 추가 되지 않도록 설정. 
            if (!attackTargets.ContainsKey(target))
                attackTargets.Add(target, target.GetComponent<Control>());
        }
    }

    public void RemoveAttackTarget(Collider target)
    {
        attackTargets.Remove(target);
    }

    public void ResetAttackTargets()
    {
        attackTargets = new Dictionary<Collider, Control>();
    }

    //public void AddFrontAttackTargets(params Collider[] targets)
    //{
    //    foreach (var target in targets)
    //    {
    //        // AttackTarget에 이미 Target이 포함되어 있으면 추가 되지 않도록 설정. 
    //        if (!frontTargets.ContainsKey(target))
    //            frontTargets.Add(target, target.GetComponent<Control>());
    //    }
    //}

    //public void RemoveFrontAttackTarget(Collider target)
    //{
    //    frontTargets.Remove(target);
    //}

    //public void ResetFrontAttackTargets()
    //{
    //    frontTargets = new Dictionary<Collider, Control>();
    //}
    #endregion

    #region Attack Control

    public virtual void StopAttack()
    {
        UseAttackEnd();
    }

    public void StartJustAttack(AttackData attackData, float attackSpeed = 1, int weight = 0, Action OnStartAttack = null, Action OnComplete = null)
    {
        currentAttack.Reset();

        if (currentAttack.attackBuffer.isRunningTimer)
            Timer.instance.ForceTimerEnd(currentAttack.attackBuffer);

        PlayAnimation(attackData, attackSpeed, weight);
        currentAttack.currentAttackAction = () => UseAttack(attackData, attackSpeed, OnStartAttack, OnComplete);

        ContinueAttack();
    }

    protected void StartAttack(AttackData attackData, float attackSpeed = 1, int weight = 0, Action OnStartAttack = null, Action OnComplete = null)
    {
        currentAttack.Reset();

        if (currentAttack.attackBuffer.isRunningTimer)
            Timer.instance.ForceTimerEnd(currentAttack.attackBuffer);

        PlayAnimation(attackData, attackSpeed, weight);    
        currentAttack.currentAttackAction = () => UseAttack(attackData, attackSpeed, OnStartAttack, OnComplete);

        ContinueAttack();
    }

    public void ContinueAttack()  
    {
        currentAttack.currentAttackAction?.Invoke();
    }

    private void PlayAnimation(AttackData attackData, float attackSpeed, int weight)
    {
        AnimationControl animationControl = control.GetModel<Model>().animationControl;  
        animationControl.ResetAnimationState();     

        //animationControl.PlayAnimationCrossFade(attackData.clip, fadeLength: 0.1f, currentAttack.currentAttackNormalizedTime, speed: attackSpeed, weight: weight);      
        animationControl.PlayAnimation(attackData.clip, currentAttack.currentAttackNormalizedTime, speed: attackSpeed, weight: weight);      
        //animationControl.PlayAnimation(attackData.clip, currentAttack.currentAttackNormalizedTime, speed: attackSpeed, weight: (int)AnimationWeight.Enemy.HpReduce);

    }

    private void UseAttack(AttackData attackData, float attackSpeed, Action OnStartAttack, Action OnComplete)            
    {
        currentAttack.attackBuffer.time = attackData.GetTotalLength() * (1 - currentAttack.currentAttackNormalizedTime) / attackSpeed;  

        UseAttackStart();
        OnStartAttack?.Invoke();  
        AddDefaultAttackDataEvent(attackData);

        float oneFrameTime = attackData.GetTotalLength() / attackData.GetTotalFrame() / attackSpeed; 
        TimerFunctions.instance.TimerFrameToFixedOneTime(currentAttack.attackBuffer, oneFrameTime,
            OnFrame: (frame) =>
            {                
                attackData.CheckCurrentAnimationFrame(frame);  
                currentAttack.currentAttackNormalizedTime = attackData.GetFrameToNomarlizedTime(frame + 1);
            },
            OnComplete: () =>
            {
                UseAttackEnd();
                OnComplete?.Invoke();
            });
    }

    #endregion

    #region Attack Data Event 설정

    protected virtual void AddDefaultAttackDataEvent(AttackData attackData)
    {
        attackData.ResetAllHandleEvent();

        attackData.AddHandleEvent(AttackDataParameterDefine.CREATE_EFFECT,
            (parameter) =>
            {
                CreateResourceManager.instance.CreateResource(this.gameObject, parameter.objectValue.name);
            });

        attackData.AddHandleEvent(AttackDataParameterDefine.SET_NONSTOP_ATTACK,
            (parameter) =>
            {
                control.SetIsPlayHitMotion(!parameter.boolValue);
            });
    }

    #endregion

    private void UseAttackStart()
    {
        currentAttack.isAttack = true;

        StopAttackWait();
    }

    private void UseAttackEnd()
    {
        // Attack이 종료되면 공격이 가능하도록 함.
        control.SetIsPlayHitMotion(true);

        currentAttack.Reset();
        AttackEnd();

        Timer.instance.TimerStop(currentAttack.attackBuffer, isReset: true);

        if (!attackWaitBuffer.isRunningTimer)
            StartAttackWait(isReset: false);
    }

    protected virtual void AttackEnd()    
    {
    }

    public virtual float GetAttackSpeed()
    {
        return control.GetStats<Stats>().manager.GetValue(StatsValueDefine.AttackSpeedRatio);
    }

    public virtual float GetAttackDamage()
    {
        StatsManager statsManager = control.GetStats<Stats>().manager;
        float damage = statsManager.GetValue(StatsValueDefine.DamageRatio); // 기본 공격력 
        return damage;
    }

    // 크리티컬 검사 후 Ratio 가져오기.
    protected float GetCriticalRatio()
    {
        return CheckAvailableCritical() ? control.GetStats<Stats>().manager.GetValue(StatsValueDefine.CriticalRatio) : 0;
    }

    // 크리티컬 검사.
    private bool CheckAvailableCritical()
    {
        RandomSetting criticalSuccess = new RandomSetting(control.GetStats<Stats>().manager.GetValue(StatsValueDefine.CriticalPercentage));
        RandomSetting result = RNGManager.instance.GetRandom(criticalSuccess);

        if (result == criticalSuccess)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // 크리티컬 검사.
    protected bool CheckAvailableCritical(float criticalPercentage)
    {
        RandomSetting criticalSuccess = new RandomSetting(criticalPercentage);
        RandomSetting result = RNGManager.instance.GetRandom(criticalSuccess);

        if (result == criticalSuccess)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    protected void ShowAttackEffect(Vector3 position, string effectName)
    {
        CreateResourceManager.instance.CreateResource(this.gameObject, effectName, position, Quaternion.identity);
    }
}
