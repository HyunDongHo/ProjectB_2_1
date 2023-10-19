using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;
using CodeStage.AntiCheat.ObscuredTypes;

public class AntiCheatRandomizeCryptoKey : Singleton<AntiCheatRandomizeCryptoKey>
{
    private WaitForSeconds randomizeRandomTotalTime = new WaitForSeconds(defaultRandomizeRandomTotalTime);

    private const float defaultRandomizeRandomTotalTime = 2.5f;
    private const int divideCount = 10;

    private Dictionary<int, List<IObscuredType>> obscuredTypes = new Dictionary<int, List<IObscuredType>>();
    private int maxDivdedObscuredType = 0;

    public void Add(IObscuredType obscuredType)
    {   
        AddDivideObscuredType();

        obscuredTypes[maxDivdedObscuredType].Add(obscuredType);

        randomizeRandomTotalTime = new WaitForSeconds(defaultRandomizeRandomTotalTime / (maxDivdedObscuredType + 1));
    }

    private void AddDivideObscuredType()
    {
        bool isFirstSet = obscuredTypes.Keys.Count == 0;

        if (isFirstSet)
        {
            AddNewObscuredTypes(maxDivdedObscuredType);

            StartCoroutine(RepeatCoroutine());
        }
        else
        {
            if ((obscuredTypes[maxDivdedObscuredType].Count + 1) / (divideCount + 1) > 0)
            {
                AddNewObscuredTypes(++maxDivdedObscuredType);
            }
        }
    }

    private void AddNewObscuredTypes(int currentAddIndex)
    {
        obscuredTypes.Add(currentAddIndex, new List<IObscuredType>());
    }

    private IEnumerator RepeatCoroutine()
    {
        int currentDivdedObscuredType = 0;
        while (true)
        {
            int totalCount = obscuredTypes[currentDivdedObscuredType].Count;
            for (int i = 0; i < totalCount; i++)
                obscuredTypes[currentDivdedObscuredType][i]?.RandomizeCryptoKey();

            if (++currentDivdedObscuredType > maxDivdedObscuredType)
                currentDivdedObscuredType = 0;

            yield return randomizeRandomTotalTime;
        }
    }
}
