using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RNG;

public class Defense : MonoBehaviour
{
    public StatsManager manager;

    public bool IsAvailableEvasion(float evasionPercentage)
    {
        RandomSetting evasionSuccess = new RandomSetting(evasionPercentage);
        RandomSetting result = RNGManager.instance.GetRandom(evasionSuccess);

        return evasionSuccess == result;
    }

    public float GetBaseEvasionPercentage()
    {
        return manager.GetValue(StatsValueDefine.DodgePercentage);
    }

    public float GetCurrentDefense()
    {
        return manager.GetValue(StatsValueDefine.Defence);
    }
}
