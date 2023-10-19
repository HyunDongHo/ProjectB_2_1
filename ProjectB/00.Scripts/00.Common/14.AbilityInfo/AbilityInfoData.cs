using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityInfoData : ScriptableObject
{
    public float maxHp = 0;
    public float maxExp = 0;

    public float damageRatio = 1;

    public float attackSpeedRatio = 1;
    public float moveSpeedRatio = 1;

    public float criticalRatio = 1;
    public float criticalPercentage = 0;

    public float dodgePercentage = 0;
    public float defence = 0;

    public float chanceToHit = 0;
    public float stunRatio = 0;
}
