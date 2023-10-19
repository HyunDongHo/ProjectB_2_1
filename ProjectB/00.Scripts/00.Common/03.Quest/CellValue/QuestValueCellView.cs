using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestValueCellView : MonoBehaviour
{
    public RectTransform rectTransform;

    public Text content;
    public Text progress;

    public void SetHeight(float height)
    {
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
    }

    public void SetYPosition(float yPosition)
    {
        rectTransform.anchoredPosition = new Vector3(rectTransform.anchoredPosition.x, yPosition);
    }

    public void SetContent(string text, Color color)
    {
        content.text = text;
        content.color = color;
    }

    public void SetProgress(string text, Color color)
    {
        progress.text = text;
        progress.color = color;
    }
}
