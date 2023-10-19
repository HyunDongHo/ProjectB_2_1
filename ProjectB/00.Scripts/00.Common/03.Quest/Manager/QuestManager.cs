using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RNG;

public class QuestManager : Singleton<QuestManager>
{
    public List<QuestData> quests = new List<QuestData>();

    public Action<QuestData> OnInit;
    public Action<QuestData> OnAdded;
    public Action<QuestData> OnUpdated;
    public Action<QuestData> OnRemoved;

    public void Init(List<QuestData> questDatas)
    {
        for (int i = 0; i < questDatas.Count; i++)
        {
            SetQuestData(questDatas[i]);

            OnInit?.Invoke(questDatas[i]);
        }
    }

    public void ExecuteAfterSceneLoad()
    {
        for (int i = 0; i < quests.Count; i++)
        {
            OnInit?.Invoke(quests[i]);
        }
    }

    #region Quest Data 설정.

    private void SetQuestData(QuestData data)
    {
        data.Init();

        foreach (QuestProgress progressValue in data.saveData.progressValues)
            data.SetTargetValue(progressValue.valueTarget, progressValue.value);

        quests.Add(data);
    }

    private void ReleaseQuestData(QuestData data)
    {
        if (!string.IsNullOrEmpty(data.nextQuestName))
            AddQuest(data.nextQuestName);
    }

    public QuestData AddQuest(string questName)
    {
        QuestData data = BackEndServerManager.instance.GetSavedResourceData<QuestData>(questName, isCopy: true);

        SetQuestData(data);
        OnAdded?.Invoke(data);

        return data;
    }

    public QuestData RemoveQuest(QuestData questData)
    {
        quests.Remove(questData);

        ReleaseQuestData(questData);
        OnRemoved?.Invoke(questData);

        return questData;
    }

    public void RemoveQuestOnly(QuestData questData)
    {
        quests.Remove(questData);
        OnRemoved?.Invoke(questData);
    }

    #endregion

    #region Quest Value 설정

    public void AddQuestVariable(string variableName, int value)
    {
        foreach (var data in quests)
        {
            foreach (var progressValue in data.saveData.progressValues)
            {
                if (progressValue.valueTarget == variableName)
                {
                    RandomSetting randomSetting = new RandomSetting(progressValue.getValueRate);
                    RandomSetting result = RNGManager.instance.GetRandom(randomSetting);

                    if (randomSetting == result)
                        data.SetTargetValue(variableName, progressValue.value + value);
                }
            }
        }
    }

    public void SetQuestVariable(string variableName, int resetValue)
    {
        foreach (var data in quests)
        {
            foreach (var progressValue in data.saveData.progressValues)
            {
                if (progressValue.valueTarget == variableName)
                    data.SetTargetValue(variableName, resetValue);
            }
        }
    }

    #endregion
}
