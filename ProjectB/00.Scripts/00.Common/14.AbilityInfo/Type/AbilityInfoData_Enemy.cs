using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Ability Data", menuName = "Custom/Ability Data/Enemy")]
public class AbilityInfoData_Enemy : AbilityInfoData
{
    public float attackMinDamage = 1;
    public float attackMaxDamage = 1;

    public int rewardMin = 0;
    public int rewardMax = 0;

    public int coreAmount = 0;

    public int expAmount = 0;
    public int goldAmount = 0;
}
