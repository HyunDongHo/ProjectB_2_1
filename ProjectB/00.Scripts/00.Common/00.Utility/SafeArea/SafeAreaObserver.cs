using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaObserver : Singleton<SafeAreaObserver>
{
    public Action<Rect> OnSafeAreaChaged;

    private Rect LastSafeArea = new Rect(0, 0, 0, 0);
    private Rect convertScreenResolution = new Rect(0, 0, 0, 0);

    private void Start()
    {
        StartCoroutine(CheckSafeAreaChanged());
    }

    public void SetResolution()
    {
        int setWidth = 1920;

        Screen.SetResolution(setWidth, (int)(((float)Screen.height / Screen.width) * setWidth), true);
    }

    private IEnumerator CheckSafeAreaChanged()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

        while (true)
        {
            Rect safeArea = Screen.safeArea;

            if (safeArea != LastSafeArea)
                ApplySafeArea(safeArea);

            yield return waitForSeconds;
        }
    }

    public void UpdateSafeArea()
    {
        Rect safeArea = Screen.safeArea;

        ApplySafeArea(safeArea);
    }

    private void ApplySafeArea(Rect rect)
    {
        LastSafeArea = rect;

        OnSafeAreaChaged?.Invoke(new Rect(rect.position.x / Screen.width, 0,
                                         (rect.position + rect.size).x / Screen.width, 1));
    }
}