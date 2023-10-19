using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterBoxSafeArea : SafeArea
{
    public RectTransform left;
    public RectTransform right;

    protected override void HandleOnSafeAreaChaged(Rect rect)
    {
        left.anchorMin = new Vector2(0, rect.y);
        left.anchorMax = new Vector2(rect.x, rect.height);

        right.anchorMin = new Vector2(rect.width, rect.y);
        right.anchorMax = new Vector2(1, rect.height);
    }
}
