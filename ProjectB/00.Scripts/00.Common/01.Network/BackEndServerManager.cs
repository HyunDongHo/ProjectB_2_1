using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LitJson;
using RNG;
using BackEnd;

public class ServerErrorDefine
{
    public const string AccessTokenError = "400"; // 기기 로컬에 액세스 토큰이 존재하지 않는데 토큰 로그인 시도를 한 경우
    public const string DifferentDeviceLogin = "401"; // 다른 기기로 로그인하여 refresh_token이 만료된 경우
    public const string GamerNotFound = "404";
}

public class BackEndServerManager : Singleton<BackEndServerManager>
{
    #region BackEnd Control Chart

    [System.Serializable]
    public class ChartData
    {
        public object dataClass;
        public JsonData jsonData;

        public string chartName;
        public int selectedChartFileId;

        public ChartData(string chartName, int selectedChartFileId, JsonData jsonData)
        {
            this.chartName = chartName;
            this.selectedChartFileId = selectedChartFileId;
            this.jsonData = jsonData;
        }
    }

    private List<ChartData> chartDatas = new List<ChartData>();
    private Dictionary<string, ScriptableObject> saveDatas = new Dictionary<string, ScriptableObject>();

    public void AddChartData(string chartName, int selectedChartFileId, Action OnChartLoaded)
    {
        BackEndFunctions.instance.GetChartToJsonData(selectedChartFileId.ToString(),
            OnSuccess: (jsonData) =>
            {
                chartDatas.Add(new ChartData(chartName, selectedChartFileId, jsonData));

                OnChartLoaded?.Invoke();
            });
    }

    public T GetChartJsonDataToValue<T>(string chartName, string rowName, int index)
    {
        ChartData chartData = chartDatas.Find(data => data.chartName == chartName);
        return Logic.ChangeStringToValue<T>(chartData.jsonData["rows"][index][rowName]["S"].ToString());
    }

    public int GetChartColumnCount(string chartName)
    {
        ChartData chartData = chartDatas.Find(data => data.chartName == chartName);

        return chartData.jsonData["rows"].Count;
    }

    public T GetSavedResourceData<T>(string resourceName, bool isCopy = false) where T : ScriptableObject
    {
        if (saveDatas.ContainsKey(resourceName))
        {
            T data = saveDatas[resourceName] as T;

            if (isCopy)
                data = Instantiate(data);

            return data;
        }
        else
        {
            Debug.LogWarning($"[ServerChartDataManager] 저장된 {resourceName} 이 존재하지 않아 ResoureManager에서 불러옵니다.");

            T data = ResourceManager.instance.Load<T>(resourceName, isCopy);

            return data;
        }
    }

    #endregion

    #region Chart Save

    public bool isAlreadySaveData = false;

    public void InitServerChartData()
    {
        if (isAlreadySaveData)
            return;

        SaveStageData();
        SaveEnemyData();
        SaveLocalizeData();

#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
#endif

        isAlreadySaveData = true;
    }

    #region StageData

    public void SaveStageData()
    {       
        foreach(var sector in SceneSettingManager.instance.NormalSectorType)
        {
            SetStageData(ResourceManager.instance.Load<StageData>(sector.Value), "A_Normal", sector.Key);
        }

        foreach (var sector in SceneSettingManager.instance.MiddleSectorType)
        {
            SetStageData(ResourceManager.instance.Load<StageData>(sector.Value), "A_Middle", sector.Key);
        }

        foreach (var sector in SceneSettingManager.instance.BossSectorType)
        {
            SetStageData(ResourceManager.instance.Load<StageData>(sector.Value), "A_Boss", sector.Key);
        }
    }

    private void SetStageData(StageData data, string targetStage, long sectorNum)
    {
        data.stageEnemyDatas = new List<StageEnemyData>();
        data.stageMiddleBossDatas = new List<StageEnemyData>();
        data.stageFinalBossDatas = new List<StageEnemyData>();
        data.stageEnemyDatasTotalMonsterCnt = new List<int>();

        for(int i=0; i < StaticManager.Backend.Chart.StageEnemyData.List.Count; ++i)
        {
            if(sectorNum != StaticManager.Backend.Chart.StageEnemyData.List[i].SectorNum)
            {
                continue;
            }

            string targetEnemy = StaticManager.Backend.Chart.StageEnemyData.List[i].TargetEnemy;
            int createCount = 0;

            switch (targetStage)
            {
                case "A_Normal":
                    createCount = StaticManager.Backend.Chart.StageEnemyData.List[i].A_Normal;
                    break;
                case "A_Middle":
                    createCount = StaticManager.Backend.Chart.StageEnemyData.List[i].A_Middle;
                    break;
                case "A_Boss":
                    createCount = StaticManager.Backend.Chart.StageEnemyData.List[i].A_Boss;
                    break;
            }

            if (createCount != 0)
            {
                switch (targetEnemy.Substring(0, 1))
                {
                    case "E":
                        GameObject enemyPrefab = ResourceManager.instance.Load<GameObject>(targetEnemy);
                        data.stageEnemyDatas.Add(new StageEnemyData(enemyPrefab, createCount));
                        data.stageEnemyDatasTotalMonsterCnt.Add(createCount);
                        break;
                    case "M":
                        GameObject middleBossPrefab = ResourceManager.instance.Load<GameObject>(targetEnemy);
                        data.stageMiddleBossDatas.Add(new StageEnemyData(middleBossPrefab, createCount));
                        break;
                    case "B":
                        GameObject bossPrefab = ResourceManager.instance.Load<GameObject>(targetEnemy);
                        data.stageFinalBossDatas.Add(new StageEnemyData(bossPrefab, createCount));
                        break;
                }
            }
        }



        //for (int i = 0; i < stageEnemyColumnCount; i++)
        //{
        //    string targetEnemy = BackEndServerManager.instance.GetChartJsonDataToValue<string>(stageEnemyChartName, "TargetEnemy", i);

        //    if (sectorNum != BackEndServerManager.instance.GetChartJsonDataToValue<int>(stageEnemyChartName, "SectorNum", i))
        //        continue;

        //    int createCount = (int)BackEndServerManager.instance.GetChartJsonDataToValue<float>(stageEnemyChartName, targetStage, i);  
        //    if (createCount != 0)
        //    {
        //        switch (targetEnemy.Substring(0, 1))
        //        {
        //            case "E":
        //                GameObject enemyPrefab = ResourceManager.instance.Load<GameObject>(targetEnemy);
        //                data.stageEnemyDatas.Add(new StageEnemyData(enemyPrefab, createCount));
        //                data.stageEnemyDatasTotalMonsterCnt.Add(createCount);
        //                break;
        //            case "M":
        //                GameObject middleBossPrefab = ResourceManager.instance.Load<GameObject>(targetEnemy);
        //                data.stageMiddleBossDatas.Add(new StageEnemyData(middleBossPrefab, createCount));
        //                break;
        //            case "B":
        //                GameObject bossPrefab = ResourceManager.instance.Load<GameObject>(targetEnemy);
        //                data.stageFinalBossDatas.Add(new StageEnemyData(bossPrefab, createCount));   
        //                break;
        //        }                
        //    }
        //}

       //string stageAdjustChartName = ChartDefine.STAGE_ADJUST_DATA;
       //
       //int stageAdjustColumnCount = BackEndServerManager.instance.GetChartColumnCount(stageAdjustChartName);
       //for (int i = 0; i < stageAdjustColumnCount; i++)
       //{
       //    string targetStage = BackEndServerManager.instance.GetChartJsonDataToValue<string>(stageAdjustChartName, "TargetStage", i);
       //
       //    if (targetStage == currentStage)
       //    {
       //        float adjustmentRatio = BackEndServerManager.instance.GetChartJsonDataToValue<float>(stageAdjustChartName, "Adjust", i);
       //        data.stageAdjustmentRatio = adjustmentRatio;
       //        break;
       //    }
       //}
        saveDatas.Add(data.name, Instantiate(data));                
    }

    #endregion

    #region EnemyData

    private void SaveEnemyData()
    {
        int columnCount = BackEndServerManager.instance.GetChartColumnCount(ChartDefine.ENEMY_DATA_CHART);
        Debug.Log(columnCount + "aaaaaaaaaaaaaaaa");  
        for (int i = 0; i < columnCount; i++)
        {
            AbilityInfoData_Enemy abilityInfo = ResourceManager.instance.Load<AbilityInfoData_Enemy>($"{BackEndServerManager.instance.GetChartJsonDataToValue<string>(ChartDefine.ENEMY_DATA_CHART, "EnemyName", i)}_AbilityInfo");

            if (abilityInfo != null)
                SetEnemyInfoData(abilityInfo, i);
        }
    }

    private void SetEnemyInfoData(AbilityInfoData_Enemy data, int index)
    {
        string chartName = ChartDefine.ENEMY_DATA_CHART;

        data.maxHp = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "MaxHp", index);

        data.attackMinDamage = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "MinDamage", index);
        data.attackMaxDamage = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "MaxDamage", index);

        data.criticalRatio = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "CriticalRatio", index);
        data.criticalPercentage = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "CriticalPercentage", index);

        data.attackSpeedRatio = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "AttackSpeedRatio", index);

        data.dodgePercentage = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "DodgePercentage", index);

        data.rewardMin = (int)BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "RewardMin", index);
        data.rewardMax = (int)BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "RewardMax", index);

        data.coreAmount = (int)BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "CoreAmount", index);

        data.expAmount = (int)BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "ExpAmount", index);
        data.goldAmount = (int)BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "GoldAmount", index);

        saveDatas.Add(data.name, data);
    }

    #endregion

    #region GambleData

    #endregion

    #region Contents

    #endregion

    #region QuestData


    #endregion

    #region EquipmentData

    #endregion

    #region PetData

    #endregion

    #region Localize Data

    private void SaveLocalizeData()
    {
        string localizeDataChart = ChartDefine.LOCALIZE_DATA;

        int columnCount = GetChartColumnCount(localizeDataChart);
        for (int i = 0; i < columnCount; i++)
        {
            string key = GetChartJsonDataToValue<string>(localizeDataChart, "Key", i);

            LocalizeData localizeData = new LocalizeData();
            localizeData.kr = GetChartJsonDataToValue<string>(localizeDataChart, "KR", i);

            LocalizeManager.instance.AddLocalizeDatas(key, localizeData);
        }
    }

    #endregion

    #region ReadChartEveryTime

    #region PlayerData
    public void SetPlayerInfoData(AbilityInfoData_Player data, int index)
    {
        index = Mathf.Clamp(index, 0, index);

        string chartName = ChartDefine.PLAYER_DATA_CHART;

        data.maxExp = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "MaxExp", index);

        data.maxHp = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "MaxHp", index);
        data.maxSp = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "MaxSp", index);

        data.hpRecovery = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "HpRecovery", index);
        data.hpRecoveryTime = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "HpRecoveryTime", index);
        data.spRecovery = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "SpRecovery", index);
        data.spRecoveryTime = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "SpRecoveryTime", index);

        data.damageRatio = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "DamageRatio", index);

        data.criticalRatio = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "CriticalRatio", index);
        data.criticalPercentage = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "CriticalPercentage", index);

        data.attackSpeedRatio = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "AttackSpeedRatio", index);
        data.moveSpeedRatio = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "MoveSpeedRatio", index);

        data.dodgePercentage = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "DodgePercentage", index);
        data.expBonusRatio = 1;
    }
    public void SetPlayerInfoData(AbilityInfoData_Player data, BackendData.Chart.PlayerData.Item playerData)
    {       
        data.maxExp = playerData.MaxExp;

        data.maxHp = playerData.MaxHp;
        data.maxSp = playerData.MaxSp;
        data.hpRecovery = playerData.HpRecovery;
        data.hpRecoveryTime = playerData.HpRecoveryTime;
        
        data.damageRatio = playerData.DamageRatio;

        data.criticalRatio = playerData.CriticalRatio;
        data.criticalPercentage = playerData.CriticalPercentage;

        data.attackSpeedRatio = playerData.AttackSpeedRatio;
        data.moveSpeedRatio = playerData.MoveSpeedRatio;
        data.dodgePercentage = playerData.DodgePercentage;
        data.expBonusRatio = 1;
    }

    #endregion

    #region PlayerEnhancementData
    private void SavePlayerEnhancementData()
    {
        string chartName = ChartDefine.PLAYER_ENHANCEMENT_DATA;

        int columnCount = BackEndServerManager.instance.GetChartColumnCount(chartName);
        Debug.Log($"enhancement chart column count : {columnCount}");
        for (int i = 0; i < columnCount; i++)
        {
            PlayerEnhancementData enhancementData = ResourceManager.instance.Load<PlayerEnhancementData>($"PlayerEnhancementData_{BackEndServerManager.instance.GetChartJsonDataToValue<string>(chartName, "StatName", i)}");
            
            if (enhancementData != null)
                SetPlayerEnhancementInfoData(enhancementData, i);
        }
    }

    private void SetPlayerEnhancementInfoData(PlayerEnhancementData data, int index)
    {
        string chartName = ChartDefine.PLAYER_ENHANCEMENT_DATA;

        data.statName = BackEndServerManager.instance.GetChartJsonDataToValue<string>(chartName, "StatName", index);
        data.maxLevel = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "MaxLevel", index);
        data.needCount = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "NeedCount", index);
        data.needCountRatio = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, "NeedCountRatio", index);
        data.statUIName = BackEndServerManager.instance.GetChartJsonDataToValue<string>(chartName, "StatUIName", index);

        saveDatas.Add(data.name, data);
    }


    #endregion


    #region EnemyData

    #endregion

    #region Upgrade

    public UpgradeData[] GetUpgradeData(UpgradeTarget upgradeTarget, int level)
    {
        string chartName = ChartDefine.PLAYER_UPGRADE;

        List<UpgradeData> upgradeDatas = new List<UpgradeData>();

        int columnCount = BackEndServerManager.instance.GetChartColumnCount(chartName);
        for (int i = 0; i < columnCount; i++)
        {
            if (upgradeTarget.ToString() == BackEndServerManager.instance.GetChartJsonDataToValue<string>(chartName, "Target", i))
            {
                UpgradeData upgradeData = new UpgradeData();

                upgradeData.level = level;

                upgradeData.targetValue = BackEndServerManager.instance.GetChartJsonDataToValue<string>(chartName, "TargetValue", i);

                if (System.Enum.TryParse(BackEndServerManager.instance.GetChartJsonDataToValue<string>(chartName, "Operation", i), out Operation operation))
                    upgradeData.operation = operation;

                upgradeData.currentValue = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, level.ToString(), i);
                upgradeData.nextValue = BackEndServerManager.instance.GetChartJsonDataToValue<float>(chartName, (level + 1).ToString(), i);

                upgradeDatas.Add(upgradeData);
            }
        }

        return upgradeDatas.ToArray();
    }

    #endregion

    #region Quest

    public QuestTextData GetQuestTextData(string questName)
    {
        string chartName = ChartDefine.QUEST_TEXT;

        QuestTextData textData = new QuestTextData();

        int totalCount = BackEndServerManager.instance.GetChartColumnCount(chartName);
        for (int i = 0; i < totalCount; i++)
        {
            if (BackEndServerManager.instance.GetChartJsonDataToValue<string>(chartName, "QuestName", i) == questName)
            {
                textData.title = BackEndServerManager.instance.GetChartJsonDataToValue<string>(chartName, "Title", i);

                textData.startQuestDialogueJson = BackEndServerManager.instance.GetChartJsonDataToValue<string>(chartName, "QuestStartDialogue", i);
                textData.endQuestDialogueJson = BackEndServerManager.instance.GetChartJsonDataToValue<string>(chartName, "QuestEndDialogue", i);

                string questValueTargetJson = BackEndServerManager.instance.GetChartJsonDataToValue<string>(chartName, "QuestValueTarget", i);
                if (!string.IsNullOrEmpty(questValueTargetJson))
                {
                    JsonData questValueTargetJsonData = JsonMapper.ToObject(questValueTargetJson);

                    int valueTargetCount = questValueTargetJsonData["valueTargets"].Count;
                    for (int valueIndex = 0; valueIndex < valueTargetCount; valueIndex++)
                    {
                        JsonData valueTarget = questValueTargetJsonData["valueTargets"][valueIndex];

                        textData.valueTexts.Add(valueTarget["target"].ToString(), valueTarget["content"].ToString());
                    }
                }

                break;
            }
        }

        return textData;
    }

    #endregion

    #region Equipment

    #endregion

    #endregion

#endregion
}
