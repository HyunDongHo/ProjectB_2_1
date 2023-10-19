using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestCell : MonoBehaviour
{
    public QuestCellView view;

    protected QuestData questData;

    public Action<QuestCell> OnClicked;

    [Space]

    public RectTransform questValueParent;
    public GameObject questValueCellPrefab;

    private List<QuestValueCell> valueCells = new List<QuestValueCell>();

    private void OnDestroy()
    {
        if(questData != null)
            Release(questData);
    }

    public virtual void Init(QuestData data)
    {
        questData = data;
        AddQuestDataEvent(data);

        StartUISet();
    }

    public virtual void Release(QuestData data)
    {
        RemoveQuestDataEvent(data);
    }

    private void AddQuestDataEvent(QuestData data)
    {
        view.button.onClick.AddListener(HandleOnCellClicked);

        data.OnValueChanged += HandleOnValueChanged;
        data.OnStateChanged += HandleOnStateChanged;
    }

    private void RemoveQuestDataEvent(QuestData data)
    {
        view.button.onClick.RemoveListener(HandleOnCellClicked);

        data.OnValueChanged -= HandleOnValueChanged;
        data.OnStateChanged -= HandleOnStateChanged;
    }

    private void HandleOnCellClicked()
    {
        OnClicked?.Invoke(this);
    }

    private void HandleOnValueChanged()
    {
        SetQuestValueCell(questData);
    }

    private void HandleOnStateChanged()
    {
        QuestState questState = questData.saveData.state;

        SetNotAccpeted(questState == QuestState.NotAccepted);
        SetInProgress(questState == QuestState.InProgress);
        SetAchieved(questState == QuestState.Achieved);

        SetHighlightBlinking(questState == QuestState.NotAccepted || questState == QuestState.Achieved);
    }

    private void StartUISet()
    {
        view.Init();
        view.SetTitleText(questData.textData.title);
        view.SetTeleportIcon(questData.IsAvailableAccessLocation());

        InitValueCells();

        HandleOnValueChanged();
        HandleOnStateChanged();
    }

    #region Cell State Control

    private void SetNotAccpeted(bool active)
    {
        view.SetNotAccepted(active);
    }

    private void SetInProgress(bool active)
    {
        foreach (var valueCell in valueCells)
            valueCell.SetCellActive(active);
    }

    private void SetAchieved(bool active)
    {
        view.SetAcheived(active);
    }

    private void SetHighlightBlinking(bool active)
    {
        view.SetHighlightBlinking(active);
    }

    #endregion

    #region Quest Value Cell

    // Quest Value Target이 두개 이상인 경우가 있기 때문에 갯수 만큼 생성해서 만들어줌.
    public void InitValueCells()
    {
        List<QuestProgress> progressValues = questData.saveData.progressValues;

        int currentIndex = 0;

        for (currentIndex = 0; currentIndex < progressValues.Count; currentIndex++)
        {
            QuestValueCell valueCell = (Instantiate(questValueCellPrefab, questValueParent, instantiateInWorldSpace: false) as GameObject).GetComponent<QuestValueCell>();

            valueCell.Init(progressValues[currentIndex].valueTarget, questValueParent.rect.height, currentIndex);
            valueCell.SetValueCell(questData.textData, progressValues[currentIndex]);

            valueCells.Add(valueCell);
        }

        if(currentIndex > 1)
            transform.GetComponent<RectTransform>().sizeDelta += Vector2.up * questValueParent.rect.height * (currentIndex - 1);
    }

    private void SetQuestValueCell(QuestData questData)
    {
        foreach (QuestProgress progressValue in questData.saveData.progressValues)
        {
            foreach (var valueCell in valueCells)
            {
                if (valueCell.GetValueTarget() == progressValue.valueTarget)
                    valueCell.SetValueCell(questData.textData, progressValue);
            }
        }
    }

    #endregion

    #region Quest Control

    public virtual void SetButtonClicked(bool isClicked)
    {
        view.SetHighlightActive(isClicked);
    }

    #endregion

    public QuestData GetQuestData()
    {
        return questData;
    }
}
