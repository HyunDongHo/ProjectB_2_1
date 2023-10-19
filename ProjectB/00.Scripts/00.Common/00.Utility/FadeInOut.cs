using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Scheduler;

public class FadeInOut : Singleton<FadeInOut>
{
    public const float FADE_IN = 0.0f;
    public const float FADE_OUT = 1.0f;

    public Image fadePanel;

    public void FadeIn(float duration, Action OnComplete = null)
    {
        FadeStart(FADE_OUT, FADE_IN, duration, OnComplete);
    }

    public void FadeOut(float duration, Action OnComplete = null)
    {
        FadeStart(FADE_IN, FADE_OUT, duration, OnComplete);
    }

    public void FadeSet(float fadeValue)
    {
        SetFadeActive(true);
        fadePanel.color = new Color(0, 0, 0, fadeValue);

        CheckFadeActive();
    }

    private void FadeStart(float startAlpha, float endAlpha, float duration, Action OnComplete)
    {
        SetFadeActive(true);
        fadePanel.color = new Color(0, 0, 0, startAlpha);

        TimerBuffer buffer = new TimerBuffer(duration);

        Timer.instance.TimerStart(buffer,
            OnFrame: () =>
            {
                fadePanel.color = new Color(0, 0, 0, Mathf.Lerp(fadePanel.color.a, endAlpha, buffer.timer / duration));
            },
            OnComplete: () =>
            {
                CheckFadeActive();
                OnComplete?.Invoke();
            });
    }

    private void CheckFadeActive()
    {
        if (fadePanel.color.a <= FADE_IN)
        {
            SetFadeActive(false);
        }
        else if (fadePanel.color.a <= FADE_OUT)
        {
            SetFadeActive(true);
        }
    }

    private void SetFadeActive(bool isActive)
    {
        fadePanel.gameObject.SetActive(isActive);
    }
}
