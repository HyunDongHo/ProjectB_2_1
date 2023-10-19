using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RNG;

[System.Serializable]
public class TreasureEnhancementSetting
{
    [System.Serializable]
    public class TreasureSettingContainer : RandomSettingContainer
    {
        public Define.EnhancementResult enhancementResult;

        public override RandomSetting GetRandomSetting()
        {
            return new RandomSetting(percentage, enhancementResult.ToString());
        }
    }

    public int level;
    public List<TreasureSettingContainer> containers = new List<TreasureSettingContainer>();
}

[CreateAssetMenu(fileName = "New Equipment Enhancement Random Data", menuName = "Custom/Random/Treasure Enchant Random Data")]
public class RandomTreasureEnchantData : RandomData
{
    public List<TreasureEnhancementSetting> containers = new List<TreasureEnhancementSetting>();

    public RandomSetting[] GetRandomSettings(int index)
    {
        RandomSetting[] rngSettings = new RandomSetting[containers[index].containers.Count];

        for (int i = 0; i < rngSettings.Length; i++)
            rngSettings[i] = containers[index].containers[i].GetRandomSetting();

        return rngSettings;
    }

    public TreasureEnhancementSetting.TreasureSettingContainer GetRandomContainer(RandomSetting[] randomSettingsToGamble, int index)
    {
        RandomSetting result = GetRandomSetting(randomSettingsToGamble);

        return containers[index].containers.Find(data => data.enhancementResult.ToString() == result.name);
    }

    public TreasureEnhancementSetting TreasureEnhancementSetting(int index)
    {
        return containers[index];
    }

    public bool IsEnchantMaxLevel(int index)
    {
        if (index >= containers.Count)
            return true;
        else
            return false;
    }
}