using System.Collections;
using System.Collections.Generic;
using RNG;
using UnityEngine;

public enum GambleResult
{
    Success,
    Fail
}

[System.Serializable]
public class GambleResultSettingContainer : RandomSettingContainer
{
    public GambleResult gambleResult;

    public override RandomSetting GetRandomSetting()
    {
        return new RandomSetting(percentage, gambleResult.ToString());
    }
}

[CreateAssetMenu(fileName = "New Gameble Result Data", menuName = "Custom/Random/Gameble Result Data")]
public class GambleResultData : RandomData
{
    public List<GambleResultSettingContainer> containers = new List<GambleResultSettingContainer>();

    public RandomSetting[] GetRandomSettings()
    {
        RandomSetting[] rngBuffers = new RandomSetting[containers.Count];

        for (int i = 0; i < rngBuffers.Length; i++)
            rngBuffers[i] = containers[i].GetRandomSetting();

        return rngBuffers;
    }

    public GambleResultSettingContainer GetRandomContainer(RandomSetting[] randomSettingsToGamble)
    {
        RandomSetting result = GetRandomSetting(randomSettingsToGamble);

        return containers.Find(data => data.gambleResult.ToString() == result.name);
    }
}
