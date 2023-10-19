using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RNG;

[System.Serializable]
public class EquipmentGambleSettingContainer : RandomSettingContainer
{
    public List<RandomItemData> gambleItemDatas = new List<RandomItemData>();

    public override RandomSetting GetRandomSetting()
    {
        return null;
    }
}

[CreateAssetMenu(fileName = "New Equipment Gameble Data", menuName = "Custom/Random/Equipment Gameble Data")]
public class EquipmentGambleData : RandomData
{
    public List<EquipmentGambleSettingContainer> containers = new List<EquipmentGambleSettingContainer>();

    public RandomSetting[] GetRandomSettings()
    {
        RandomSetting[] rngBuffers = new RandomSetting[containers.Count];

        for (int i = 0; i < rngBuffers.Length; i++)
            rngBuffers[i] = containers[i].GetRandomSetting();

        return rngBuffers;
    }

    public EquipmentGambleSettingContainer GetRandomContainer(RandomSetting[] randomSettingsToGamble)
    {
        return null;
    }

    public RandomItemData GetGambleItemData()
    {
        EquipmentGambleSettingContainer container = GetRandomContainer(GetRandomSettings());
        RandomItemData randomItemData = container.gambleItemDatas[Random.Range(0, container.gambleItemDatas.Count)];

        return randomItemData;
    }
}
