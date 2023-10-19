using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RNG;

[System.Serializable]
public class RandomSettingContainer
{
    public float percentage = 0;

    public virtual RandomSetting GetRandomSetting()
    {
        return new RandomSetting(percentage);
    }
}

public abstract class RandomData : ScriptableObject
{
    protected RandomSetting GetRandomSetting(RandomSetting[] randomSetting)
    {
        RandomSetting result = RNGManager.instance.GetRandom(randomSetting);

        return result;
    }
}
