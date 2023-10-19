using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class Hp : MonoBehaviour
{
    public StatsManager manager;

    public Action<float> OnHpInit;

    public Action<float> OnHpSet;

    public Action<float> OnHpAdd;

    public Action<float> OnHpReduce;
    public GameObject lastAffectObject { get; set; }

    public bool isAvailableReduceHp { get; set; } = true;


    private float currentHp = 0;

    private float maxHp = 0;

    float defaultTDEnemyMaxHp = 500;
    public bool isAlive
    {
        get
        {
            return currentHp > 0;
        }
    }

    public float MaxHp
    {
        get
        {
            return maxHp;
        }
    }

    /* Init */
    public void InitHp(float hp)
    {
        currentHp = hp;

        OnHpInit?.Invoke(currentHp);
    }

    /* Set Hp Max*/
    public void SetHpToMax()
    {
        float maxHp = manager.GetValue(StatsValueDefine.MaxHp);
        
        AddHp(maxHp - currentHp);
    }
    public void SetHpToMax(EnemyType enemyType) // 풀피 채우는 
    {
        float maxHp = 0;
        switch (enemyType)
        {
            case EnemyType.Normal:
                maxHp = Define.Util.GetExpressionValue(Define.ExpressionType.NormalHP, StaticManager.Backend.GameData.PlayerGameData.NowStageLevel);
                break;
            case EnemyType.Boss:
                maxHp = Define.Util.GetExpressionValue(Define.ExpressionType.BossHP, StaticManager.Backend.GameData.PlayerGameData.NowStageLevel);  
                break;    

        }  
        AddHp(maxHp - currentHp, enemyType);      
    }
    public void SetHpMax(EnemyType enemyType) // 스테이지 몬스터 용 max Hp 설정 
    {
        float maxHp = 0;
        switch (enemyType)
        {
            case EnemyType.Normal:
                maxHp = Define.Util.GetExpressionValue(Define.ExpressionType.NormalHP, StaticManager.Backend.GameData.PlayerGameData.NowStageLevel);
                break;
            case EnemyType.Boss:
                maxHp = Define.Util.GetExpressionValue(Define.ExpressionType.BossHP, StaticManager.Backend.GameData.PlayerGameData.NowStageLevel);
                break;

        }
        AddHp(enemyType);
    }

    public void SetHpMax_TrainingDungeon(PlayerType playerType, long level) //던전용  Max HP 설정
    {
        float trainingDungeonEnemyMaxHp = 0;
        //float defaultTDEnemyMaxHp = 300;
        switch (playerType)
        {
            case PlayerType.Warrior:
                trainingDungeonEnemyMaxHp = Define.Util.GetExpressionValue(Define.ExpressionType.WarriorTrainingHP, level);
                break;
            case PlayerType.Archer:
                trainingDungeonEnemyMaxHp = Define.Util.GetExpressionValue(Define.ExpressionType.ArcherTrainingHP, level);
                break;
            case PlayerType.Wizard:
                trainingDungeonEnemyMaxHp = Define.Util.GetExpressionValue(Define.ExpressionType.WizardTrainingHP, level);    
                break;
        }
        maxHp = trainingDungeonEnemyMaxHp;
        if (maxHp == 0)
            maxHp = defaultTDEnemyMaxHp;
        Debug.Log($"던전 보스 maxHp : {maxHp}");
        AddHp(playerType, level);
        Debug.Log($"던전 보스 currentHp : {currentHp}");              
    }
    //public void SetHpToMax(int playerType)
    //{
    //    //float maxHp = manager.GetValue(StatsValueDefine.MaxHp);
    //    double defaultHp = manager.GetValue(StatsValueDefine.MaxHp); // 기본 체력 : 550
    //    double finalHp = 0;

    //    switch (playerType)
    //    {
    //        case (int)PlayerType.Warrior:
    //            finalHp = defaultHp + GetStatHp(StaticManager.Backend.GameData.PlayerGameData.DWarriorGoldStatLevel);
    //            //Debug.Log($"final {Enum.GetName(typeof(PlayerType), playerType)} Hp : {finalHp}");
    //            break;
    //        case (int)PlayerType.Archer:
    //            finalHp = defaultHp + GetStatHp(StaticManager.Backend.GameData.PlayerGameData.DArcherGoldStatLevel);
    //            //Debug.Log($"final {Enum.GetName(typeof(PlayerType), playerType)} Hp : {finalHp}");
    //            break;
    //        case (int)PlayerType.Wizard:
    //            finalHp = defaultHp + GetStatHp(StaticManager.Backend.GameData.PlayerGameData.DWizardGoldStatLevel);
    //            //Debug.Log($"final {Enum.GetName(typeof(PlayerType), playerType)} Hp : {finalHp}");
    //            break;
    //    }

    //    /* 용병 HP 추가 */
    //    finalHp = finalHp * (1 + (float)GetPartnerHp());

    //    AddHp((float)finalHp - currentHp, playerType);
    //    //AddHp(maxHp - currentHp);
    //}
    public void SetHpToMax(int playerType) // 플레이어 HP full 로 채우기 
    {
        //float maxHp = manager.GetValue(StatsValueDefine.MaxHp);
        double defaultMaxHp = GetDefaultHP(playerType); // 기본 체력 : 550
        AddHp((float)defaultMaxHp - currentHp, playerType);
       
    }

    public void SetHpMax(int playerType) // 플레이어 MaxHP 설정
    {
        //float maxHp = manager.GetValue(StatsValueDefine.MaxHp);
        double defaultMaxHp = GetDefaultHP(playerType); // 기본 체력 : 550
        OnHpSet?.Invoke(currentHp);
    }

    double GetPartnerHp() // 보유 효과 (Get)
    {
        List<BackendData.GameData.PartnerData> partnerDatas = null;
        partnerDatas = StaticManager.Backend.GameData.PlayerPartner.PartnerList;
        if (partnerDatas.Count == 0)
            return 0;

        double GetHps = 0;

        for (int i = 0; i < partnerDatas.Count; i++)
        {
            BackendData.Chart.Partner.Item chartItem = null;
            chartItem = StaticManager.Backend.Chart.Partner.GetChartALlItem().Find(item => item.PartnerID == partnerDatas[i].PartnerID);
            double _chartGrowingGetHp = chartItem.GrowingGetHp;
            double level = partnerDatas[i].PartnerLevel;
            double _getHp = _chartGrowingGetHp * level;  
            GetHps += _getHp;
        }

        return GetHps;
    }
    /* Set Hp */
    public void SetHp(float hp)
    {
        currentHp = hp;

        if (currentHp < 0)
            currentHp = 0;

        OnHpSet?.Invoke(currentHp);
    }

    /* Add Hp */
    public void AddHp(float amount)
    {
        float maxHp = manager.GetValue(StatsValueDefine.MaxHp);
        if (currentHp + amount >= maxHp)
        {
            SetHp(maxHp);
        }
        else
        {
            SetHp(currentHp + amount);
        }

        OnHpAdd?.Invoke(currentHp);
    }
    public void AddHp(float amount, int playerType)
    {
        //float maxHp = manager.GetValue(StatsValueDefine.MaxHp);
        double defaultMaxHp = GetDefaultHP(playerType); // 기본 체력 : 550  

        if (currentHp + amount >= defaultMaxHp)
        {
            SetHp((float)defaultMaxHp);
        }
        else
        {
            SetHp(currentHp + amount);  
        }

        OnHpAdd?.Invoke(currentHp);
    }
    public void AddHp(float amount, EnemyType enemyType)
    {
        float defaultMaxHp = 0;
        switch (enemyType)
        {
            case EnemyType.Normal:
                defaultMaxHp = Define.Util.GetExpressionValue(Define.ExpressionType.NormalHP, StaticManager.Backend.GameData.PlayerGameData.NowStageLevel);
                break;
            case EnemyType.Boss:
                defaultMaxHp = Define.Util.GetExpressionValue(Define.ExpressionType.BossHP, StaticManager.Backend.GameData.PlayerGameData.NowStageLevel);
                break;

        }

        if (currentHp + amount >= defaultMaxHp)
        {
            SetHp((float)defaultMaxHp);
        }
        else
        {
            SetHp(currentHp + amount);
        }

        //Debug.Log($"current {Enum.GetName(typeof(PlayerType), playerType)} hp : {currentHp}");  
        OnHpAdd?.Invoke(currentHp);
    }
    public void AddHp(EnemyType enemyType)
    {
        float defaultMaxHp = 0;
        switch (enemyType)
        {
            case EnemyType.Normal:
                defaultMaxHp = Define.Util.GetExpressionValue(Define.ExpressionType.NormalHP, StaticManager.Backend.GameData.PlayerGameData.NowStageLevel);
                break;
            case EnemyType.Boss:
                defaultMaxHp = Define.Util.GetExpressionValue(Define.ExpressionType.BossHP, StaticManager.Backend.GameData.PlayerGameData.NowStageLevel);
                break;

        }
        maxHp = defaultMaxHp;
        if (maxHp == 0)
        {
            maxHp = 123; // 500 
            SetHp((float)123);
            Debug.Log("스테이지 몬스터 max Hp 오류 ");
        }
        else
        {
            SetHp((float)defaultMaxHp); 
        }

        //Debug.Log($"current {Enum.GetName(typeof(PlayerType), playerType)} hp : {currentHp}");  
        OnHpAdd?.Invoke(currentHp);
    }

    public void AddHp(PlayerType playerType, long level) // 던전용 
    {
        float trainingDungeonEnemyMaxHp = 0;
        //float defaultTDEnemyMaxHp = 300;
        switch (playerType)
        {
            case PlayerType.Warrior:
                trainingDungeonEnemyMaxHp = Define.Util.GetExpressionValue(Define.ExpressionType.WarriorTrainingHP, level);
                break;
            case PlayerType.Archer:
                trainingDungeonEnemyMaxHp = Define.Util.GetExpressionValue(Define.ExpressionType.ArcherTrainingHP, level);
                break;
            case PlayerType.Wizard:
                trainingDungeonEnemyMaxHp = Define.Util.GetExpressionValue(Define.ExpressionType.WizardTrainingHP, level);
                break;
        }
        maxHp = trainingDungeonEnemyMaxHp;
        if (maxHp == 0)
        {
            maxHp = defaultTDEnemyMaxHp;
            SetHp((float)defaultTDEnemyMaxHp);

        }
        else
        {
            SetHp((float)trainingDungeonEnemyMaxHp);  
        }

        //Debug.Log($"current {Enum.GetName(typeof(PlayerType), playerType)} hp : {currentHp}");  
        OnHpAdd?.Invoke(currentHp);
    }

    //public void AddHp(float amount, int playerType)
    //{
    //    //float maxHp = manager.GetValue(StatsValueDefine.MaxHp);
    //    double defaultHp = manager.GetValue(StatsValueDefine.MaxHp); // 기본 체력 : 550  
    //    double finalHp = 0;

    //    switch (playerType)
    //    {
    //        case (int)PlayerType.Warrior:
    //            finalHp = defaultHp + GetStatHp(StaticManager.Backend.GameData.PlayerGameData.DWarriorGoldStatLevel);
    //            break;
    //        case (int)PlayerType.Archer:
    //            finalHp = defaultHp + GetStatHp(StaticManager.Backend.GameData.PlayerGameData.DArcherGoldStatLevel);
    //            break;
    //        case (int)PlayerType.Wizard:
    //            finalHp = defaultHp + GetStatHp(StaticManager.Backend.GameData.PlayerGameData.DWizardGoldStatLevel);
    //            break;
    //    }
    //    if (currentHp + amount >= finalHp)
    //    {
    //        SetHp((float)finalHp);
    //    }
    //    else
    //    {
    //        SetHp(currentHp + amount);
    //    }

    //    Debug.Log($"current {Enum.GetName(typeof(PlayerType), playerType)} hp : {currentHp}");
    //    OnHpAdd?.Invoke(currentHp);
    //}

    /* Reduce Hp */
    public bool ReduceHp(GameObject affectObject, float amount)
    {
        if (currentHp > 0 && isAvailableReduceHp)
        {
            lastAffectObject = affectObject;
 
            SetHp(currentHp - amount);
            OnHpReduce?.Invoke(currentHp);

            return true;
        }

        return false;
    }

    public void ForceDie()
    {
        if(currentHp > 0 && isAlive == true)
        {
            currentHp = 0;
            SetHp(0);
            OnHpReduce?.Invoke(0);
        }
    }

    /* Get */
    public float GetCurrentHp()
    {
        return currentHp;
    }


    /* Get Hp Rate */
    public float GetCurrentHpRate()
    {
        return currentHp / manager.GetValue(StatsValueDefine.MaxHp);
    }
    //public float GetCurrentHpRate()
    //{
    //    return currentHp / manager.GetValue(StatsValueDefine.MaxHp);
    //}

    public double GetDefaultHP(int playerType) // 변하지 않는 기본 공격 여기다 추가 
    {
        //double defaultHp = manager.GetValue(StatsValueDefine.MaxHp); // 기본 체력 : 550  
        double defaultHp = 550; // 기본 체력 : 550  

        double finalHp = 0;

        finalHp = defaultHp + StaticManager.Backend.GameData.PlayerGameData.GetStatRatio(Define.StatType.HpRatio);

        finalHp = finalHp * (1 + (float)(GetPartnerHp() 
                                + StaticManager.Backend.GameData.PlayerTreasure.GetTreasureRatio(Define.StatType.HpRatio) 
                                + StaticManager.Backend.GameData.PlayerDice.GetDiceRatio(Define.StatType.HpRatio)
                                + (float)StaticManager.Backend.GameData.PlayerAdsBuff.GetAdsBuffRatio(BackendData.Chart.AdsBuff.AdsType.Hp)
                                + StaticManager.Backend.GameData.PlayerGameData.GetPlayerUpgradeRatio(playerType, Define.StatType.HpRatio)));
        maxHp = (float)finalHp;
        return finalHp;  
    }


}
