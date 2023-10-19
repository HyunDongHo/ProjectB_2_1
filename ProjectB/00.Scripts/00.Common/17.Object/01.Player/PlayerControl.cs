using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;
using RNG;

public class PlayerControl : Control
{
    public PlayerUtility utility;

    public Action<PlayerControl> OnHpExhausted;
    public Action<PlayerControl> OnHpExhaustedImmediately;

    private const int DEFAULT_HIT_MOTION_SKIP_START_FRAME = 2;

    protected bool isPlayHitMotion = false;

    protected override void Awake()
    { 

        base.Awake();

        utility.Init(this);
    }

    protected override void Start()
    {

        //PlayerStats stats = StageManager.instance.playerControl.GetStats<PlayerStats>();

        //if (stats.hp.GetCurrentHp() <= 0)
        //    stats.hp.SetHpToMax();

        PlayerStats stats = GetStats<PlayerStats>();

        if (stats.hp.GetCurrentHp() <= 0)
            stats.hp.SetHpToMax((int)GetAttack<PlayerAttack>().playerModelType);  


        StartCoroutine(HpRecovery());  
        //StartCoroutine(SpRecovery());
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        utility.Release();
    }

    #region 이벤트 관리

    protected override void AddEvent()
    {
        PlayerStats stats = GetStats<PlayerStats>();
        if (stats != null)
        {
            stats.hp.OnHpReduce += HandleOnHpReduce;
            stats.level.OnLevelInit += HandleOnLevelSet;
            stats.level.OnLevelUp += HandleOnLevelUp;
            utility.modelSetter.OnModelChanged += HandleOnModelChanged;

        }


        //stats.hp.OnHpReduce += HandleOnHpReduce;
        //stats.level.OnLevelInit += HandleOnLevelSet;
        //stats.level.OnLevelUp += HandleOnLevelUp;
        //utility.modelSetter.OnModelChanged += HandleOnModelChanged;

        //utility.equipment.OnWeaponEquipEnd += HandleOnWeaponEquipEnd;
        //utility.equipment.OnPetEquipEnd += HandleOnPetEquipEnd;
    }

    protected override void RemoveEvent()
    {
        PlayerStats stats = GetStats<PlayerStats>();
        if (stats != null)
        {
            stats.hp.OnHpReduce -= HandleOnHpReduce;

            stats.level.OnLevelInit -= HandleOnLevelSet;
            stats.level.OnLevelUp -= HandleOnLevelUp;


            utility.modelSetter.OnModelChanged -= HandleOnModelChanged;

        }


        //stats.hp.OnHpReduce -= HandleOnHpReduce;

        //stats.level.OnLevelInit -= HandleOnLevelSet;
        //stats.level.OnLevelUp -= HandleOnLevelUp;


        //utility.modelSetter.OnModelChanged -= HandleOnModelChanged;

        //utility.equipment.OnWeaponEquipEnd -= HandleOnWeaponEquipEnd;
        //utility.equipment.OnPetEquipEnd -= HandleOnPetEquipEnd;
    }

    #endregion

    #region HP 컨트롤 관련

    private void HandleOnHpReduce(float hp)
    {
        if (hp <= 0)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            HpExhausted();
        }
    }

    private void HpExhausted()
    {
        ResetState(false);

        GetModel<PlayerModel>().ResetAnimationState();
        GetModel<PlayerModel>().PlayDieAnimation(() => OnHpExhausted?.Invoke(this));
        OnHpExhaustedImmediately?.Invoke(this);
    }

    public void ResetState(bool active)
    {
        PlayerMove move = GetMove<PlayerMove>();
        PlayerAttack attack = GetAttack<PlayerAttack>();

        isEnabledControl = active;

        GetComponent<CapsuleCollider>().enabled = active;

        move.ResetMove();
        move.isAvaliableUpdateMove = active;

        attack.CompleteAttackWait();
        attack.StopAttack();
        attack.isEnableAttack = active;
    }

    #endregion

    #region Level Data 설정

    // Level Set이 되었을 때는 Data만 교체 함.
    private void HandleOnLevelSet(int level)
    {
        ChangeLevelData(level);  
    }

    // Level Up 하였을 때 해당 Level의 Data로 교체 후 Stats를 초기화함.
    private void HandleOnLevelUp(int level)
    {
        ChangeLevelData(level);

        PlayerStats stats = GetStats<PlayerStats>();
        stats.hp.SetHpToMax((int)GetAttack<PlayerAttack>().playerModelType);
        //Debug.Log($"hp : {stats.hp.GetCurrentHp()}"); 
        Debug.Log("level up");  
        //stats.sp.SetSpToMax();
    }

    private void ChangeLevelData(int level)
    {
        PlayerStats stats = GetStats<PlayerStats>();

        stats.UpdateLevelData(level);
    }

    #endregion

    #region 자동 회복

    private IEnumerator HpRecovery()
    {
        PlayerStats stats = GetStats<PlayerStats>();

        Hp hp = stats.hp;

        while (true)
        {
            if (isEnabledControl)
            {
                float hpRecoveryTime = stats.manager.GetValue(PlayerStatsValueDefine.HpRecoveryTime);
                float hpRecoveryAmount = stats.manager.GetValue(PlayerStatsValueDefine.HpRecovery);

                yield return hpRecoveryTime > 0 ? new WaitForSeconds(hpRecoveryTime) : null;


                //if (hp.GetCurrentHp() <= stats.manager.GetValue(PlayerStatsValueDefine.MaxHp))
                //    hp.AddHp(hpRecoveryAmount);
                int modelType = (int)GetAttack<PlayerAttack>().playerModelType;
                float defaultHP = (float)hp.GetDefaultHP(modelType);
                if (hp.GetCurrentHp() <= defaultHP)
                {
                    hp.AddHp(hpRecoveryAmount, modelType);
                }

                //Debug.Log($"GetCurrentHp : {hp.GetCurrentHp()}");
                //Debug.Log($"hp.GetDefaultHP : {hp.GetDefaultHP((int)GetAttack<PlayerAttack>().playerModelType)}");  
                //Debug.Log($"hpRecoveryAmount : {hpRecoveryAmount}");  
            }
            else
                yield return null;
        }
    }
   
    //private IEnumerator SpRecovery()
    //{
    //    PlayerStats stats = GetStats<PlayerStats>();

    //    Hp hp = stats.hp;
    //    PlayerSp sp = stats.sp;

    //    while (true)
    //    {
    //        if (isEnabledControl)
    //        {
    //            float spRecoveryTime = stats.manager.GetValue(PlayerStatsValueDefine.SpRecoveryTime);
    //            float spRecoveryAmount = stats.manager.GetValue(PlayerStatsValueDefine.SpRecovery);

    //            yield return spRecoveryTime > 0 ? new WaitForSeconds(spRecoveryTime) : null;

    //            if (sp.GetCurrentSp() <= stats.manager.GetValue(PlayerStatsValueDefine.MaxSp))
    //                sp.AddSp(spRecoveryAmount);
    //        }
    //        else
    //            yield return null;
    //    }
    //}

    #endregion

    #region 피격 

    protected override void GotAttacked(bool isPlayHitMotion, int hitMotionSkipStartFrame)
    {
        if (!isEnabledControl || !isAvailableControl) return;

      //  ShaderFunction.instance.Play_VMToon_Hit(GetModel<PlayerModel>(), 0.5f);

        if (isPlayHitMotion)
            GotHitted(hitMotionSkipStartFrame);
    }

    private void GotHitted(int hitSkip)
    {
       // StartHitMotion();

        PlayerMove move = GetMove<PlayerMove>();
        PlayerModel model = GetModel<PlayerModel>();

        float hitTotalTime = model.animationControl.GetTotalTime(PlayerAnimationType.HIT);
        float hitTotalFrame = model.animationControl.GetTotalFrame(PlayerAnimationType.HIT);
        float oneFrameTime = hitTotalTime / hitTotalFrame;

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
        GetModel<PlayerModel>().PlayHitAnimation(OnEnd: () => GetModel<PlayerModel>().PlayCurrentSetIdleAnimation());

        isPlayHitMotion = true;

        PlayerAttack playerAttack = GetAttack<PlayerAttack>();
        playerAttack.StopAttack();
        playerAttack.ResetAttackValue();
    }

    private void UpdateHitMotion(TimerBuffer timerBuffer, int frame, int hitMotionSkipStartFrame)
    {
        PlayerAttack playerAttack = GetAttack<PlayerAttack>();

        if (InputManager.instance.GetMouseButtonDown(0, isCheckOverlapCanvas: true))
        {
            EndHitMotion(timerBuffer);

            playerAttack.CompleteAttackWait();
        }
        else if (frame >= hitMotionSkipStartFrame && playerAttack.IsAvaliableAttack())
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
        float baseValue = base.CalculateHitPercentage(attackerDamage, defenderDefense, attackerLevel, defenderLevel);

        baseValue = Logic.GetDebuffValueAccordingToLevelDifference(
            value: baseValue,
            levelDifference: Mathf.Abs(attackerLevel - defenderLevel),
            endLevelDifference: 5,
            0.025f, 0.05f, 0.1f, 0.2f, 0.3f);

        return baseValue;
    }

    protected override float CalculateEvasionPercentage(int attackerLevel, int defenderLevel)
    {
        float baseValue = base.CalculateEvasionPercentage(attackerLevel, defenderLevel);

        baseValue = Logic.GetDebuffValueAccordingToLevelDifference(
            value: baseValue,
            levelDifference: Mathf.Abs(attackerLevel - defenderLevel),
            endLevelDifference: 5,
            0.025f, 0.05f, 0.1f, 0.2f, 0.3f);

        return baseValue;
    }

    protected override float CalculateDamageReduceRatio(int divideDamage, float attackerDamage, float defenderDefense, int attackerLevel, int defenderLevel)
    {
        float baseValue = base.CalculateDamageReduceRatio(divideDamage, attackerDamage, defenderDefense, attackerLevel, defenderLevel);

        baseValue = Logic.GetDebuffValueAccordingToLevelDifference(
            value: baseValue,
            levelDifference: Mathf.Abs(attackerLevel - defenderLevel),
            endLevelDifference: 5,
            0.025f, 0.05f, 0.1f, 0.2f, 0.3f);

        return baseValue;
    }

    #endregion

    protected void SetMove(bool isCanMove)
    {
        PlayerMove move = GetMove<PlayerMove>();

        move.isAvaliableUpdateMove = isCanMove;
        move.isAvailableMove = isCanMove;
    }

    protected override int GetInitHitMotionSkipStartFrame()
    {
        return DEFAULT_HIT_MOTION_SKIP_START_FRAME;
    }

    private void HandleOnModelChanged()
    {
        utility.weaponModel.ChangeWeaponToCurrentPlayerModel();
    }
    public void PlayBoundAnimation(Action OnEnd)
    {
        GetModel<Model>().animationControl.PlayAnimation("Bound", isSameAniAvailablePlay: false, weight:(int)PlayerAnimationWeight.Bound, OnAnimationEnd: OnEnd);
    }
}
