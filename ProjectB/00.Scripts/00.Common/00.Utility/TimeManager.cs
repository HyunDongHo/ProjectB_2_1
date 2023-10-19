using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TimeManager : Singleton<TimeManager>
{
    public bool isChangeTimeScale = false;

    public void SetTimeScale(float changeTimeScale, float duration)
    {
        if (isChangeTimeScale == true)
            return;

        isChangeTimeScale = true;
        Time.timeScale = changeTimeScale;
        StartCoroutine(CoResetTimeScale(duration));
    }

    IEnumerator CoResetTimeScale(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
        isChangeTimeScale = false;
    }
}
