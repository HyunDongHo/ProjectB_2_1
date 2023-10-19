using System;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Ability Data", menuName = "Custom/Ability Data/Player")]
public class AbilityInfoData_Player : AbilityInfoData
{
    public float maxSp = 0;

    public float spRecovery = 2.5f;
    public float spRecoveryTime = 0.5f;

    public float spAttackRecoverAmount = 10;

    public float spManualReduceAmount = 5;
    public float spAutoReduceAmount = 10;

    public float spDrainRatioPerHit = 0;

    public float hpRecovery = 1.5f;
    public float hpRecoveryTime = 0.5f;

    public float hpDrainRatioPerHit = 0;
    public float goldDropBonusRatio = 1;
    public float expBonusRatio = 1;

    [System.Serializable]
    public class SkillData
    {
        public float skillDamageRatio = 0;
        public float skillCoolTime = 0;
        public float skillSpConsum = 0;
    }
    public SkillData[] skillDatas = new SkillData[PlayerSkillUseCheck.TOTAL_USE_SKILL_COUNT];
    public float skillCoolDecreaseRatio = 0;
}
