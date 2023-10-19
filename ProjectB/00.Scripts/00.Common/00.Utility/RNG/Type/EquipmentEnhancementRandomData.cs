using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RNG;

[System.Serializable]
public class EquipmentEnhancementSetting
{
    [System.Serializable]
    public class EnhancementSettingContainer : RandomSettingContainer
    {
      //  public EnhancementResult enhancementResult;

        public override RandomSetting GetRandomSetting()
        {
            return null;// new RandomSetting(percentage, enhancementResult.ToString());
        }
    }

    public int charge;
    public List<EnhancementSettingContainer> containers = new List<EnhancementSettingContainer>();
}

[CreateAssetMenu(fileName = "New Equipment Enhancement Random Data", menuName = "Custom/Random/Equipment Enhancement Random Data")]
public class EquipmentEnhancementRandomData : RandomData
{    
    public List<EquipmentEnhancementSetting> containers = new List<EquipmentEnhancementSetting>();

    public RandomSetting[] GetRandomSettings(int index)
    {
        RandomSetting[] rngSettings = new RandomSetting[containers[index].containers.Count];

        for (int i = 0; i < rngSettings.Length; i++)
            rngSettings[i] = containers[index].containers[i].GetRandomSetting();

        return rngSettings;
    }

    public EquipmentEnhancementSetting.EnhancementSettingContainer GetRandomContainer(RandomSetting[] randomSettingsToGamble, int index)
    {
        RandomSetting result = GetRandomSetting(randomSettingsToGamble);

        return null;// containers[index].containers.Find(data => data.enhancementResult.ToString() == result.name);
    }

    public EquipmentEnhancementSetting EquipmentEnhancementSetting(int index)
    {
        return containers[index];
    }
}