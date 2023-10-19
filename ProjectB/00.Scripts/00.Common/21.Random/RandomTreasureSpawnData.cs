using System.Collections;
using System.Collections.Generic;
using RNG;
using UnityEngine;

[System.Serializable]
public class RandomTreasureSpawnContainer : RandomSettingContainer
{
    public int itemID;

    public override RandomSetting GetRandomSetting()
    {
        return new RandomSetting(percentage, itemID.ToString());
    }
}

[CreateAssetMenu(fileName = "New Gameble Result Data", menuName = "Custom/Random/RandomTreasureSpawn Data")]
public class RandomTreasureSpawnData : RandomData
{
    public List<RandomTreasureSpawnContainer> containers = new List<RandomTreasureSpawnContainer>();

    public RandomSetting[] GetRandomSettings()
    {
        RandomSetting[] rngBuffers = new RandomSetting[containers.Count];

        for (int i = 0; i < rngBuffers.Length; i++)
            rngBuffers[i] = containers[i].GetRandomSetting();

        return rngBuffers;
    }

    public RandomTreasureSpawnContainer GetRandomContainer(RandomSetting[] randomSettingsToGamble)
    {
        RandomSetting result = GetRandomSetting(randomSettingsToGamble);

        return containers.Find(data => data.itemID.ToString() == result.name);
    }
}
