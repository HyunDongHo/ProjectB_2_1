using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;

public class PlayerControl_GamePlay : PlayerControl
{

    public PlayerRaycast_GamePlay pRaycast;

    private Dictionary<ScreenRectType, bool> preAttackCheck = new Dictionary<ScreenRectType, bool>() { { ScreenRectType.Left, false }, { ScreenRectType.Right, false } };

    private TimerBuffer attackEndDelay = new TimerBuffer(0.1f);

    protected bool isAttackAuto = false;
    public bool isAttackForceFrameEnd { get; protected set; } = true;

    protected bool isEndGamePlay = false;

    //public Action<PlayerControl> OnPlayerHpExhausted;

    [SerializeField]
    int activeExpRatio;
    [SerializeField]
    int notActiveExpRatio;
    protected override void AddEvent()
    {
        base.AddEvent();

        utility.skillUseCheck.OnUsedSkill += HandleOnUseSkill;

        if((StageManager.instance as GamePlayManager) == null)  
            return;

        (StageManager.instance as GamePlayManager).enemyManager.OnAnnounceEnemyDie += HandleOnAnnounceEnemyDie;

        PlayerAttack attack = GetAttack<PlayerAttack>();

        attack.OnAttackForceQuit += HandleOnAttackForceQuit;
        attack.OnAttackDataStarted += HandleOnAttackStarted;
        attack.OnAttackHitted += HandleOnAttackHitted;
        attack.OnAttackForceFrameEnd += HandleOnComboForceFrameEnd;
        attack.OnAttackDataEnd += HandleOnAttackEnd;
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();

        utility.skillUseCheck.OnUsedSkill -= HandleOnUseSkill;

        if((StageManager.instance as GamePlayManager) == null)
            return;


        (StageManager.instance as GamePlayManager).enemyManager.OnAnnounceEnemyDie -= HandleOnAnnounceEnemyDie;

        PlayerAttack attack = GetAttack<PlayerAttack>();
        attack.OnAttackForceQuit -= HandleOnAttackForceQuit;
        attack.OnAttackDataStarted -= HandleOnAttackStarted;
        attack.OnAttackHitted -= HandleOnAttackHitted;
        attack.OnAttackForceFrameEnd -= HandleOnComboForceFrameEnd;
        attack.OnAttackDataEnd -= HandleOnAttackEnd;
    }

    protected void HandleOnUseSkill(int skillIndex, bool isAuto)
    {
        if (!isEnabledControl || !isAvailableControl || GetMove<Move>().isNowBound == true) return;

        if (!GetStats<PlayerStats>().hp.isAlive) return;

       // if (SpCheck(skillIndex))
        {
            if (CheckAvailableUseSkill(skillIndex))  
            {
                if (GetAttack<PlayerAttack_GamePlay>().Attack_Skill(skillIndex))
                {
                    utility.skillUseCheck.UsedSkill(skillIndex);
                  //  GetStats<PlayerStats>().sp.AddSp(-GetSpRemoveAmount(skillIndex));
                    //pRaycast.attackRangeRaycast.radius = 0.8f;
                }
            }
        }
        //else
        //{
        //    if (!isAuto)
        //        AnnounceManager.instance.ShowAnnounce("스킬을 사용할 SP가 충분하지 않습니다.");
        //}
    }

    protected void HandleOnAnnounceEnemyDie(EnemyControl enemyControl)
    {
        // 마지막으로 영향을 준 오브젝트가 나면 처리.
        if (enemyControl.GetStats<EnemyStats>().hp.lastAffectObject == gameObject)          
        {
            // 적이 완전히 사라지고 나서 체크 하기 위해 한 프레임 뒤에 실행.
            Timer.instance.TimerStart(new TimerBuffer(Time.deltaTime), OnComplete: () => KilledEnemy(enemyControl));
        }
    }

    #region 적 처치

    protected virtual void KilledEnemy(EnemyControl enemyControl)
    {
        ResetAttackAfterKillEnemy();

        QuestDataEventManager.instance.PlayerKilledEnemy(enemyControl);
        GetEnemyReward(enemyControl, (int)GetAttack<PlayerAttack>().playerModelType);

    }

    private void ResetAttackAfterKillEnemy()
    {
       // ResetPreAttackCheck();
       //
       // PlayerAttack_GamePlay attack = GetAttack<PlayerAttack_GamePlay>();
       // attack.CompleteAttackWait();
       //// attack.ResetAttackValue();  

        pRaycast.targetCollider = null; 
    }
    private void GetEnemyReward(EnemyControl enemyControl, int playerModelType)
    {
        StatsManager enemyStatsManager = enemyControl.GetStats<Stats>().manager;
        //GetStats<PlayerStats>().exp.AddExp(enemyStatsManager.GetValue(EnemyStatsValueDefine.ExpAmount) * GetStats<Stats>().manager.GetValue(PlayerStatsValueDefine.ExpBonusRatio));
        PlayersControlManager.instance.SetPlayersExp((int)GetAttack<PlayerAttack>().playerModelType, enemyControl, activeExpRatio, notActiveExpRatio);

        /*# 퀘스트 */
        if (QuestsManager.instance.questType == BackendData.Chart.Quest.QuestType.DefeatEnemy)
            QuestsManager.instance.UpdateQuestUI();

        /* 미션 : 몬스터 처치 */
        QuestsManager.instance.UpdateMissionUI(BackendData.Chart.Mission.MissionContentType.MonsterDefeat, BackendData.Chart.Mission.MissionType.Daily, 1);
        QuestsManager.instance.UpdateMissionUI(BackendData.Chart.Mission.MissionContentType.MonsterDefeat, BackendData.Chart.Mission.MissionType.Repeat, 1);

        /* 광고 버프 */
        AdsBuffManager.instance.UpdateAdsBuffUI(1);

        // utility.belongings.playerWallet.AddGold((int)(enemyStatsManager.GetValue(EnemyStatsValueDefine.GoldAmount) * GetStats<Stats>().manager.GetValue(PlayerStatsValueDefine.GoldDropBonusRatio)));
        //double defaultGoldAmount = enemyStatsManager.GetValue(EnemyStatsValueDefine.GoldAmount);

        double defaultGoldAmount = Define.Util.GetExpressionValue(Define.ExpressionType.GetGold, StaticManager.Backend.GameData.PlayerGameData.NowStageLevel);
        double finalGoldAmount = defaultGoldAmount;

        finalGoldAmount *= (1 + StaticManager.Backend.GameData.PlayerGameData.GetStatRatio(Define.StatType.GoldGetRatio)  
                            + StaticManager.Backend.GameData.PlayerTreasure.GetTreasureRatio(Define.StatType.GoldGetRatio) 
                            + (float)StaticManager.Backend.GameData.PlayerDice.GetDiceRatio(Define.StatType.GoldGetRatio)
                            + (float)StaticManager.Backend.GameData.PlayerAdsBuff.GetAdsBuffRatio(BackendData.Chart.AdsBuff.AdsType.Gold));
        
        //1) 골드 획득 적용
        RewardManager.instance.ShowRewardWindow(new List<int>() { 10001 }, new List<double>() { finalGoldAmount }, false);  
        //StaticManager.Backend.GameData.PlayerGameData.UpdateUserData((int)PlayerType.None, finalGoldAmount);
        //StageManager.instance.canvasManager.GetUIManager<UIManager_Common>().RefreshCommonUI();

        //StaticManager.Backend.GameData.PlayerGameData.UpdateUserData((int)PlayerType.None, (enemyStatsManager.GetValue(EnemyStatsValueDefine.GoldAmount) * GetStats<Stats>().manager.GetValue(PlayerStatsValueDefine.GoldDropBonusRatio)), 0);
        //utility.belongings.playerWallet.AddCore((int)(enemyStatsManager.GetValue(EnemyStatsValueDefine.CoreAmount)));

        UI_ItemDropAchivement itemComponent = StageManager.instance.canvasManager.GetUIManager<UI_ItemDropAchivement>();
        // 2) 골드 UI획득
        itemComponent.CreateDropItem(StaticManager.Backend.Chart.Item.GetItem(10001), finalGoldAmount);

        Dictionary<int, double> ResultDropItems = ItemDropManager.instance.NormalStageDropItem();
        if (ResultDropItems == null || ResultDropItems.Count == 0)
            return;

        // 3) 아이템 획득 적용   
        List<int> itemIds = new();
        List<double> itemCounts = new();
        foreach (var list in ResultDropItems)
        {
            itemIds.Add(list.Key);
            itemCounts.Add(list.Value);
            // 4) 아이템 UI획득
            itemComponent.CreateDropItem(StaticManager.Backend.Chart.Item.GetItem(list.Key), list.Value);
        }
        RewardManager.instance.ShowRewardWindow(itemIds, itemCounts, false);

      
        //  foreach (var rewardDataSetting in enemyControl.enemyReward.rewardDataSettings)    
        {
            // RewardData rewardData = rewardDataSetting.GetRewardData();

        }
    }

    #endregion

    #region 플레이어 공격 컨트롤

    protected virtual void HandleOnAttackForceQuit()
    {
        isAttackForceFrameEnd = true;

        if (!GetAttack<Attack>().isEnableAttack)
            GetAttack<Attack>().isEnableAttack = true;

        SetMove(true);
    }

    protected virtual void HandleOnAttackStarted(PlayerAttackType attackType)
    {
        Timer.instance.TimerStop(attackEndDelay);

        isAttackForceFrameEnd = false;

        PlayerMove move = GetMove<PlayerMove>();
        switch (attackType)
        {
            case PlayerAttackType.Auto:
                if (isAttackAuto)
                    GetAttack<Attack>().isEnableAttack = false;
                SetMove(false);
                break;
            case PlayerAttackType.Combo:
                SetMove(false);
                break;
            case PlayerAttackType.Skill:
                SetMove(false);
                break;
        }
    }

    protected virtual void HandleOnAttackHitted(PlayerAttackType attackType)
    {
        PlayerMove move = GetMove<PlayerMove>();
        switch (attackType)
        {
            case PlayerAttackType.Dash:
                SetMove(false);
                break;
        }
    }

    protected virtual void HandleOnComboForceFrameEnd(PlayerAttackType attackType)
    {
        isAttackForceFrameEnd = true;

        switch (attackType)
        {
            case PlayerAttackType.Auto:
                if (!GetAttack<Attack>().isEnableAttack)
                    GetAttack<Attack>().isEnableAttack = true;
                break;
            case PlayerAttackType.Skill:
                if (!GetMove<Move>().isNowBound)
                    GetModel<PlayerModel>().PlayCurrentSetIdleAnimation();
                break;
        }
    }

    protected virtual void HandleOnAttackEnd(PlayerAttackType attackType)
    {
        isAttackForceFrameEnd = false;

        if (!GetMove<Move>().isNowBound)
            GetModel<PlayerModel>().PlayCurrentSetIdleAnimation();

        Timer.instance.TimerStop(attackEndDelay);
        Timer.instance.TimerStart(attackEndDelay, OnComplete: () => SetMove(true));
    }

    #endregion

    #region Attack 체크

    protected void StartAttackCheck(Control control = null)
    {
        if (isPlayHitMotion) return;

        PlayerAttack_GamePlay attack = GetAttack<PlayerAttack_GamePlay>();

        //CheckInputPreAttack();

        float attackRecoverySp = GetStats<PlayerStats>().manager.GetValue(PlayerStatsValueDefine.SpAttackRecoveryAmount);
        attack.Attack_Auto(attackRecoverySp);
    }

    private bool CheckAttackScreenRectAndStartAttack(ScreenRectType currentScreenRectType, ScreenRectType checkScreenRectType, bool isAuto)
    {
        if (preAttackCheck.ContainsValue(true) && preAttackCheck.ContainsKey(currentScreenRectType))
            if (!preAttackCheck[currentScreenRectType])
                return false;

        if (currentScreenRectType == checkScreenRectType || preAttackCheck[checkScreenRectType])
        {
            return true;
        }
        return false;
    }

    #endregion

    #region PreAttack 설정.

    private void CheckInputPreAttack()
    {
        if (InputManager.instance.GetMouseButtonDown(0, ScreenRectType.Left, isCheckOverlapCanvas: true))
        {
            SetTogglePreAttackCheck(ScreenRectType.Left);
        }
        else if (InputManager.instance.GetMouseButtonDown(0, ScreenRectType.Right, isCheckOverlapCanvas: true))
        {
            SetTogglePreAttackCheck(ScreenRectType.Right);
        }
    }

    protected void SetTogglePreAttackCheck(ScreenRectType screenRectType)
    {
        ResetPreAttackCheck();

        SetPreAttack(screenRectType, true);
    }

    protected bool ResetAfterCheckPreAttack(ScreenRectType screenRectType)
    {
        bool preAttack = preAttackCheck[screenRectType];

        SetPreAttack(screenRectType, false);

        return preAttack;
    }

    private void SetPreAttack(ScreenRectType screenRectType, bool isPreAttack)
    {
        preAttackCheck[screenRectType] = isPreAttack;
    }

    protected void ResetPreAttackCheck()
    {
        preAttackCheck[ScreenRectType.Left] = false;
        preAttackCheck[ScreenRectType.Right] = false;
    }

    #endregion

    #region Attack 할 때의 필요한 값 가져오기.

    public bool SpCheck(bool isAuto)
    {
        float removeAmount = GetSpRemoveAmount(isAuto);

        return GetStats<PlayerStats>().sp.IsAvailiableRemoveCombo(removeAmount);
    }

    public float GetSpRemoveAmount(bool isAuto)
    {
        PlayerStats stats = GetStats<PlayerStats>();

        return isAuto ? stats.manager.GetValue(PlayerStatsValueDefine.SpAutoReduceAmount) : stats.manager.GetValue(PlayerStatsValueDefine.SpManualReduceAmount);
    }

    public bool SpCheck(int skillIndex)
    {
        float removeAmount = GetSpRemoveAmount(skillIndex);

        return GetStats<PlayerStats>().sp.IsAvailiableRemoveCombo(removeAmount);
    }

    public float GetSpRemoveAmount(int skillIndex)
    {
        PlayerStats stats = GetStats<PlayerStats>();

        return stats.manager.GetValue(PlayerStatsValueDefine.skillStats.GetSkillSpConsum(skillIndex));
    }

    private ScreenRectType GetAutoComboScreenRectType()
    {
        if (isAttackAuto)
        {
            ScreenRectType randomScreenRectType = (ScreenRectType)UnityEngine.Random.Range(0, (int)ScreenRectType.All);

            return randomScreenRectType;
        }

        return ScreenRectType.All;
    }

    #endregion

    public void SetAttackAuto(bool isOn)
    {
        isAttackAuto = isOn;

        ResetPreAttackCheck();
    }

    protected virtual bool CheckAvailableUseSkill(int skillIndex)
    {
        bool available = true;

        // 스킬 1번과 2번은 근접이기 때문에 Attack에 들어오지 않으면 사용 불가.
        Collider[] attack = pRaycast.attackRaycast.GetRaycastHit();
        if ((skillIndex == 0 || skillIndex == 1) && attack.Length <= 0)
            available = false;

        return available;
    }

    public virtual void EndGamePlay()
    {
        isEnabledControl = false;
        isEndGamePlay = true;
    }

    public void ResetGamePlay()
    {
        isEndGamePlay = false;
    }
}
