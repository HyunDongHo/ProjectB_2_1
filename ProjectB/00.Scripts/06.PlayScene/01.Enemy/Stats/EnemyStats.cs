using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsValueDefine : StatsValueDefine
{
    public const string BaseAttackDamageMin = "BaseAttackDamageMin";
    public const string BaseAttackDamageMax = "BaseAttackDamageMax";

    public const string RewardMin = "RewardMin";
    public const string RewardMax = "RewardMax";

    public const string CoreAmount = "CoreAmount";

    public const string ExpAmount = "ExpAmount";
    public const string GoldAmount = "GoldAmount";
}

public class EnemyStats : Stats
{
    public override void SetBaseStats()
    {
        base.SetBaseStats();

        AbilityInfoData_Enemy abilityInfoData_Enemy = abilityInfoData as AbilityInfoData_Enemy;

        SetBase(EnemyStatsValueDefine.BaseAttackDamageMin, abilityInfoData_Enemy.attackMinDamage, (data) => abilityInfoData_Enemy.attackMinDamage = data);
        SetBase(EnemyStatsValueDefine.BaseAttackDamageMax, abilityInfoData_Enemy.attackMaxDamage, (data) => abilityInfoData_Enemy.attackMinDamage = data);

        SetBase(EnemyStatsValueDefine.RewardMin, abilityInfoData_Enemy.rewardMin, (data) => abilityInfoData_Enemy.attackMinDamage = data);
        SetBase(EnemyStatsValueDefine.RewardMax, abilityInfoData_Enemy.rewardMax, (data) => abilityInfoData_Enemy.attackMinDamage = data);

        SetBase(EnemyStatsValueDefine.CoreAmount, abilityInfoData_Enemy.coreAmount, (data) => abilityInfoData_Enemy.coreAmount = (int)data);

        SetBase(EnemyStatsValueDefine.ExpAmount, abilityInfoData_Enemy.expAmount, (data) => abilityInfoData_Enemy.expAmount = (int)data);
        SetBase(EnemyStatsValueDefine.GoldAmount, abilityInfoData_Enemy.goldAmount, (data) => abilityInfoData_Enemy.goldAmount = (int)data);
    }
}
