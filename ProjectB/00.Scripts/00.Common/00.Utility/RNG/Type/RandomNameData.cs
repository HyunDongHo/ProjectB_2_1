using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RNG;

[System.Serializable]
public class RandomNameSettingContiner : RandomSettingContainer
{
    public string name;

    public override RandomSetting GetRandomSetting()
    {
        return new RandomSetting(percentage, name);
    }
}

[CreateAssetMenu(fileName = "New Random Name Data", menuName = "Custom/Random/Random Name Data")]
public class RandomNameData : RandomData
{
    public List<RandomNameSettingContiner> containers = new List<RandomNameSettingContiner>();

    public RandomSetting[] GetRandomSettings()
    {
        RandomSetting[] rngBuffers = new RandomSetting[containers.Count];

        for (int i = 0; i < rngBuffers.Length; i++)
            rngBuffers[i] = containers[i].GetRandomSetting();

        return rngBuffers;
    }

    public RandomNameSettingContiner GetRandomContainer(RandomSetting[] randomSettingsToGamble)
    {
        RandomSetting result = GetRandomSetting(randomSettingsToGamble);

        return containers.Find(data => data.name == result.name);
    }
}

