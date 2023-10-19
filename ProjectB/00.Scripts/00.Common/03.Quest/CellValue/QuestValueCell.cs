using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestValueCell : MonoBehaviour
{
    public QuestValueCellView view;

    private string valueTarget;

    public void Init(string valueTarget, float cellHeight, int cellIndex)
    {
        this.valueTarget = valueTarget;

        view.SetHeight(cellHeight);
        view.SetYPosition(-cellHeight * cellIndex);
    }

    public void SetCellActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void SetValueCell(QuestTextData questTextData, QuestProgress questProgress)
    {
        // 퀘스트가 완료된 Value는 회색으로 변함. 이외 색깔은 하얀색.
        Color progressColor = questProgress.value >= questProgress.maxValue ? Color.gray : Color.white;

        view.SetContent(questTextData.GetContentText(valueTarget), progressColor);
        view.SetProgress(string.Format("({0}/{1})", questProgress.value, questProgress.maxValue), progressColor);
    }

    public string GetValueTarget()
    {
        return valueTarget;
    }
}
