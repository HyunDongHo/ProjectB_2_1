using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RNG;

[System.Serializable]
public class EquipmentOptionSettingContainer : RandomNameSettingContiner
{
    public string operation;
    public int selectedIndex;
}

public class EquipmentOptionData : RandomData
{
    public List<EquipmentOptionSettingContainer> containers = new List<EquipmentOptionSettingContainer>();

    public RandomSetting[] GetRandomSettings()
    {
        RandomSetting[] rngBuffers = new RandomSetting[containers.Count];

        for (int i = 0; i < rngBuffers.Length; i++)
            rngBuffers[i] = new RandomSetting(containers[i].percentage, containers[i].name);

        return rngBuffers;
    }

    public EquipmentOptionSettingContainer GetRandomContainer(RandomSetting[] randomSettingsToGamble)
    {
        RandomSetting result = GetRandomSetting(randomSettingsToGamble);

        return containers.Find(data => data.name == result.name);
    }
}
