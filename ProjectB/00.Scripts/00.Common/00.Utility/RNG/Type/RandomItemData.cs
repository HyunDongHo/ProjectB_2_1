using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RNG;

[System.Serializable]
public class RandomItemSettingContiner : RandomSettingContainer
{
  //  public ItemData itemData;

    //public override RandomSetting GetRandomSetting()
    //{
    //    return new RandomSetting(percentage, itemData.name);
    //}
}

[CreateAssetMenu(fileName = "New Random Item Data", menuName = "Custom/Random/Random Item Data")]
public class RandomItemData : RandomData
{
    public List<RandomItemSettingContiner> containers = new List<RandomItemSettingContiner>();

    public RandomSetting[] GetRandomSettings()
    {
        RandomSetting[] rngBuffers = new RandomSetting[containers.Count];

        for (int i = 0; i < rngBuffers.Length; i++)
            rngBuffers[i] = containers[i].GetRandomSetting();

        return rngBuffers;
    }

    //public RandomItemSettingContiner GetRandomContainer(RandomSetting[] randomSettingsToGamble)
    //{
    //    RandomSetting result = GetRandomSetting(randomSettingsToGamble);

    //    return containers.Find(data => data.itemData.itemName == result.name);
    //}
}