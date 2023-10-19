using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsValueDefine
{
    public const string MaxHp = "MaxHp";
    public const string MaxExp = "MaxExp";

    public const string DamageRatio = "DamageRatio";

    public const string CriticalRatio = "CriticalRatio";
    public const string CriticalPercentage = "CriticalPercentage";

    public const string AttackSpeedRatio = "AttackSpeedRatio";
    public const string MoveSpeedRatio = "MoveSpeedRatio";

    public const string DodgePercentage = "DodgePercentage";
    public const string Defence = "Defence";

    public const string ChanceToHit = "ChanceToHit";

    public const string StunRatio = "StunRatio";
}

public abstract class Stats : MonoBehaviour
{
    private List<string> addedBaseEvent = new List<string>();

    public StatsManager manager;

    public AbilityInfoData abilityInfoData;
    public Defense defense;
    //public Hp hp;
    //public Exp exp;
    //public Level level;

    public Level level;
    public Hp hp;
    public Exp exp;


    private void Awake()
    {
        abilityInfoData = Instantiate(abilityInfoData);
    }

    public virtual void SetBaseStats()
    {
        SetBase(StatsValueDefine.MaxHp, abilityInfoData.maxHp, (data) => abilityInfoData.maxHp = data);

        SetBase(StatsValueDefine.MaxExp, abilityInfoData.maxExp, (data) => abilityInfoData.maxExp = data);

        SetBase(StatsValueDefine.DamageRatio, abilityInfoData.damageRatio, (data) => abilityInfoData.damageRatio = data);

        SetBase(StatsValueDefine.CriticalRatio, abilityInfoData.criticalRatio, (data) => abilityInfoData.criticalRatio = data);
        SetBase(StatsValueDefine.CriticalPercentage, abilityInfoData.criticalPercentage, (data) => abilityInfoData.criticalPercentage = data);

        SetBase(StatsValueDefine.AttackSpeedRatio, abilityInfoData.attackSpeedRatio, (data) => abilityInfoData.attackSpeedRatio = data);
        SetBase(StatsValueDefine.MoveSpeedRatio, abilityInfoData.moveSpeedRatio, (data) => abilityInfoData.moveSpeedRatio = data);

        SetBase(StatsValueDefine.DodgePercentage, abilityInfoData.dodgePercentage, (data) => abilityInfoData.dodgePercentage = data);
        SetBase(StatsValueDefine.Defence, abilityInfoData.defence, (data) => abilityInfoData.defence = data);

        SetBase(StatsValueDefine.ChanceToHit, abilityInfoData.chanceToHit, (data) => abilityInfoData.chanceToHit = data);
        SetBase(StatsValueDefine.StunRatio, abilityInfoData.stunRatio, (data) => abilityInfoData.stunRatio = data);
    }

    protected void SetBase(string targetValue, float baseValue, Action<float> OnBaseValueChanged)
    {
        manager.SetValue(targetValue, baseValue);

        if(!addedBaseEvent.Exists(data => data == targetValue))
        {
            manager.GetStatsValue(targetValue).OnBaseValueChanged += (value) => HandleOnBaseValueChanged(value, OnBaseValueChanged);

            addedBaseEvent.Add(targetValue);
        }
    }

    protected void HandleOnBaseValueChanged(float value, Action<float> OnBaseValueChanged)
    {
        OnBaseValueChanged?.Invoke(value);
    }

    protected void SetNewAbilityInfoData(AbilityInfoData newAbilityInfoData)
    {
        abilityInfoData = newAbilityInfoData;

        SetBaseStats();
    }
}
