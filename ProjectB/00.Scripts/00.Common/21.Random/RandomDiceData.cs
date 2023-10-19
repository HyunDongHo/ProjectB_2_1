using System.Collections;
using System.Collections.Generic;
using RNG;
using UnityEngine;

[System.Serializable]
public class RandomDiceSpawnContainer : RandomSettingContainer
{
    public int itemID;

    public override RandomSetting GetRandomSetting()
    {
        return new RandomSetting(percentage, itemID.ToString());
    }
}

[CreateAssetMenu(fileName = "New Gameble Result Data", menuName = "Custom/Random/RandomDice Data")]
public class RandomDiceData : RandomData
{
    public List<RandomDiceSpawnContainer> containers = new List<RandomDiceSpawnContainer>();

    public RandomSetting[] GetRandomSettings()
    {
        RandomSetting[] rngBuffers = new RandomSetting[containers.Count];

        for (int i = 0; i < rngBuffers.Length; i++)
            rngBuffers[i] = containers[i].GetRandomSetting();

        return rngBuffers;
    }

    public RandomDiceSpawnContainer GetRandomContainer(RandomSetting[] randomSettingsToGamble)
    {
        RandomSetting result = GetRandomSetting(randomSettingsToGamble);

        return containers.Find(data => data.itemID.ToString() == result.name);
    }
}
