using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsValueDefine : StatsValueDefine
{
    public const string MaxSp = "MaxSp";

    public const string SpRecovery = "SpRecovery";
    public const string SpRecoveryTime = "SpRecoveryTime";

    public const string SpAttackRecoveryAmount = "SpAttackRecoverAmount";

    public const string SpAutoReduceAmount = "SpAutoReduceAmount";
    public const string SpManualReduceAmount = "SpManualReduceAmount";

    public const string SpDrainRatioPerHit = "SpDrainRatioPerHit";

    public const string HpRecovery = "HpRecovery";
    public const string HpRecoveryTime = "HpRecoveryTime";

    public const string HpDrainRatioPerHit = "HpDrainRatioPerHit";

    public const string GoldDropBonusRatio = "GoldDropBonusRatio";
    public const string ExpBonusRatio = "ExpBonusRatio";

    public class SkillStats
    {
        private const string SkillCoolTime = "SkillCoolTime";
        private const string SkillDamageRatio = "SkillDamageRatio";
        private const string SkillSpConsum = "SkillSpConsum";

        public string GetSkillCoolTime(int index)
        {
            return $"{SkillCoolTime}_{index + 1}";
        }
        public string GetSkillDamageRatio(int index)
        {
            return $"{SkillDamageRatio}_{index + 1}";
        }
        public string GetSkillSpConsum(int index)
        {
            return $"{SkillSpConsum}_{index + 1}";
        }
    }
    public static SkillStats skillStats = new SkillStats();

    public const string SkillCoolDecreaseRatio = "SkillCoolDecreaseRatio";
}

public class PlayerStats : Stats
{
    public PlayerSp sp;

    private void Awake()
    {
        AddEvent();
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    private void AddEvent()
    {
        UpgradeManager.instance.OnSetUpgrade += HandleOnSetUpgrade;
    }

    private void RemoveEvent()
    {
        UpgradeManager.instance.OnSetUpgrade -= HandleOnSetUpgrade;
    }

    public override void SetBaseStats()
    {
        base.SetBaseStats();

        AbilityInfoData_Player abilityInfoData_Player = abilityInfoData as AbilityInfoData_Player;

        SetBase(PlayerStatsValueDefine.MaxSp, abilityInfoData_Player.maxSp, (data) => abilityInfoData_Player.maxSp = data);  

        SetBase(PlayerStatsValueDefine.SpRecovery, abilityInfoData_Player.spRecovery, (data) => abilityInfoData_Player.spRecovery = data);
        SetBase(PlayerStatsValueDefine.SpRecoveryTime, abilityInfoData_Player.spRecoveryTime, (data) => abilityInfoData_Player.spRecoveryTime = data);

        SetBase(PlayerStatsValueDefine.SpAttackRecoveryAmount, abilityInfoData_Player.spAttackRecoverAmount, (data) => abilityInfoData_Player.spAttackRecoverAmount = data);

        SetBase(PlayerStatsValueDefine.SpAutoReduceAmount, abilityInfoData_Player.spAutoReduceAmount, (data) => abilityInfoData_Player.spAutoReduceAmount = data);
        SetBase(PlayerStatsValueDefine.SpManualReduceAmount, abilityInfoData_Player.spManualReduceAmount, (data) => abilityInfoData_Player.spManualReduceAmount = data);

        SetBase(PlayerStatsValueDefine.SpDrainRatioPerHit, abilityInfoData_Player.spDrainRatioPerHit, (data) => abilityInfoData_Player.spDrainRatioPerHit = data);

        SetBase(PlayerStatsValueDefine.HpRecovery, abilityInfoData_Player.hpRecovery, (data) => abilityInfoData_Player.hpRecovery = data);
        SetBase(PlayerStatsValueDefine.HpRecoveryTime, abilityInfoData_Player.hpRecoveryTime, (data) => abilityInfoData_Player.hpRecoveryTime = data);

        SetBase(PlayerStatsValueDefine.HpDrainRatioPerHit, abilityInfoData_Player.hpDrainRatioPerHit, (data) => abilityInfoData_Player.hpDrainRatioPerHit = data);

        //SetBase(PlayerStatsValueDefine.MoveSpeedRatio, abilityInfoData_Player.moveSpeedRatio, (data) => abilityInfoData_Player.moveSpeedRatio = data);

        SetBase(PlayerStatsValueDefine.GoldDropBonusRatio, abilityInfoData_Player.goldDropBonusRatio, (data) => abilityInfoData_Player.goldDropBonusRatio = data);
        SetBase(PlayerStatsValueDefine.ExpBonusRatio, abilityInfoData_Player.expBonusRatio, (data) => abilityInfoData_Player.expBonusRatio = data);

        for (int i = 0; i < abilityInfoData_Player.skillDatas.Length; i++)
        {
            AbilityInfoData_Player.SkillData skillData = abilityInfoData_Player.skillDatas[i];

            SetBase(PlayerStatsValueDefine.skillStats.GetSkillCoolTime(i), skillData.skillCoolTime, (data) => skillData.skillCoolTime = data);
            SetBase(PlayerStatsValueDefine.skillStats.GetSkillDamageRatio(i), skillData.skillDamageRatio, (data) => skillData.skillDamageRatio = data);
            SetBase(PlayerStatsValueDefine.skillStats.GetSkillSpConsum(i), skillData.skillSpConsum, (data) => skillData.skillSpConsum = data);
        }

        SetBase(PlayerStatsValueDefine.SkillCoolDecreaseRatio, abilityInfoData_Player.skillCoolDecreaseRatio, (data) => abilityInfoData_Player.skillCoolDecreaseRatio = data);
    }

    private void HandleOnSetUpgrade(UpgradeData upgradeData)
    {
        AdditionValue additionValue = new AdditionValue($"Upgrade_{upgradeData.targetValue.ToString()}", upgradeData.operation, upgradeData.currentValue);

        if(additionValue.operation == Operation.BaseSet)
        {
            manager.SetValue(upgradeData.targetValue, additionValue.value);
        }
        else
        {
            manager.RemoveAdditionValue(upgradeData.targetValue, additionValue.additionName, additionValue.operation, additionValue.value);
            manager.SetAdditionValue(upgradeData.targetValue, additionValue);
        }
    }

    //public void SetEquipmentOptions(EquipmentItemData itemData)
    //{
    //    SaveEquipmentItemData data = itemData.saveItemData as SaveEquipmentItemData;
    //    foreach (var option in data.options)
    //    {
    //        if (string.IsNullOrEmpty(option?.targetValue)) continue;

    //        option.additionValue.additionName = $"EquipmentOption_{option.targetValue}";

    //        if (option.additionValue.operation == Operation.BaseSet)
    //        {
    //            manager.SetValue(option.targetValue, option.additionValue.value);
    //        }
    //        else
    //        {
    //            manager.RemoveAdditionValue(option.targetValue, option.additionValue.additionName, option.additionValue.operation, option.additionValue.value);
    //            manager.SetAdditionValue(option.targetValue, option.additionValue);
    //        }
    //    }
    //}

    //public void RemoveEquipmentOptions(EquipmentItemData itemData)
    //{
    //    SaveEquipmentItemData data = itemData.saveItemData as SaveEquipmentItemData;
    //    foreach (var option in data.options)
    //    {
    //        if (string.IsNullOrEmpty(option?.targetValue)) continue;

    //        option.additionValue.additionName = $"EquipmentOption_{option.targetValue}";

    //        manager.RemoveAdditionValue(option.targetValue, option.additionValue.additionName, option.additionValue.operation, option.additionValue.value);
    //    }
    //}

    public void UpdateLevelData(int level)
    {
        BackendData.Chart.PlayerData.Item playerItem = null;
        /* # Player_Data level 1만 쓸것임 */
        StaticManager.Backend.Chart.PlayerData.Dictionary.TryGetValue(1, out playerItem);
        //StaticManager.Backend.Chart.PlayerData.Dictionary.TryGetValue(level, out playerItem);  

        if (playerItem == null)
            return;

        BackEndServerManager.instance.SetPlayerInfoData(abilityInfoData as AbilityInfoData_Player, playerItem);
        SetNewAbilityInfoData(abilityInfoData);
    }
}
