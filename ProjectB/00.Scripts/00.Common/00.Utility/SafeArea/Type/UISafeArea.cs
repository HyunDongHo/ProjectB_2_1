using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISafeArea : SafeArea
{
    private RectTransform rectArea;

    protected override void Awake()
    {
        rectArea = GetComponent<RectTransform>();

        base.Awake();
    }

    protected override void HandleOnSafeAreaChaged(Rect rect)
    {
        rectArea.anchorMin = new Vector2(rect.x, rect.y);
        rectArea.anchorMax = new Vector2(rect.width, rect.height);
    }
}
