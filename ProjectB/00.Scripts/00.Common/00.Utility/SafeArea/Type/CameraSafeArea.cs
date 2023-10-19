using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSafeArea : SafeArea
{
    private Camera rectArea;

    protected override void Awake()
    {
        rectArea = GetComponent<Camera>();

        base.Awake();
    }

    protected override void HandleOnSafeAreaChaged(Rect rect)
    {
        rectArea.rect = rect;
    }

    private void OnPreCull()
    {
        GL.Clear(true, true, Color.black);
    }
}
