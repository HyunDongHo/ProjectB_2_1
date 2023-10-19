using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestSubject
{
    Main,
    Sub
}

public enum QuestState
{
    NotAccepted,
    InProgress,
    Achieved
}

[System.Serializable]
public class QuestProgress
{
    public string valueTarget;

    [HideInInspector] public int getValueRate;
    [HideInInspector] public int value;
    public int maxValue;
}

[System.Serializable]
public class QuestSaveData
{
    public List<QuestProgress> progressValues = new List<QuestProgress>();

    public QuestState state = QuestState.NotAccepted;
}

[System.Serializable]
public class QuestTextData
{
    public string title;

    public Dictionary<string, string> valueTexts { get; private set; } = new Dictionary<string, string>();

    [Multiline] public string startQuestDialogueJson = string.Empty;
    [Multiline] public string endQuestDialogueJson = string.Empty;

    public string GetContentText(string valueTarget)
    {
        if (valueTexts.ContainsKey(valueTarget))
            return valueTexts[valueTarget];
        else
            return string.Empty;
    }
}

[CreateAssetMenu(fileName = "New QuestData", menuName = "Custom/QuestData")]
public class QuestData : ScriptableObject
{
    public string questName;

    public QuestSubject questSubject;

    public QuestSaveData saveData = new QuestSaveData();
    public QuestTextData textData = new QuestTextData();

    public string progressLocation;
    public string nextQuestName;

    [Space]

    public List<RewardDataSetting> rewardDataSettings = new List<RewardDataSetting>();

    [Space]

    public Action OnValueChanged;
    public Action OnStateChanged;

    private void OnEnable()
    {
        foreach (var rewardDataSetting in rewardDataSettings)
            rewardDataSetting.Init();
    }

    public void Init()
    {
        SetState(saveData.state);

        SetText();
    }

    public bool SetTargetValue(string valueTarget, int value)
    {
        if (IsAvailableCheckSetValue())
        {
            if (saveData.progressValues.Count > 0)
            {
                QuestProgress progress = saveData.progressValues.Find(data => data.valueTarget == valueTarget);
                SetValue(progress, value);

                CheckSetState(progress);

                return true;
            }
        }

        return false;
    }

    private bool IsAvailableCheckSetValue()
    {
        return saveData.state == QuestState.InProgress;
    }

    private void CheckSetState(QuestProgress progress)
    {
        if (progress.value >= progress.maxValue)
        {
            SetState(QuestState.Achieved);
        }
    }

    private void SetValue(QuestProgress progress, int value)
    {
        progress.value = value > progress.maxValue ? progress.maxValue : value;

        OnValueChanged?.Invoke();
    }

    public void SetState(QuestState questState)
    {
        saveData.state = questState;

        OnStateChanged?.Invoke();
    }

    private void SetText()
    {
        textData = BackEndServerManager.instance.GetQuestTextData(questName);
    }

    public bool IsAvailableAccessLocation()
    {
        return !string.IsNullOrEmpty(GetQuestProgressLocation());
    }

    public string GetQuestProgressLocation()
    {
        return progressLocation;
    }

    public string GetNextQuestName()
    {
        return nextQuestName;
    }
}
