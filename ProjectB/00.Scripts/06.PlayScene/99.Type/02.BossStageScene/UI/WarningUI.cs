using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WarningUI : MonoBehaviour
{
    public GameObject warning;

    public Vector3 warningAppearStartScale = Vector3.one;
    public Vector3 warningAppearEndScale = Vector3.one;

    public float appearScaleDuration = 0.5f;
    public float afterAppearWaitTime = 0.5f;

    [Space]

    public Vector3 warningDisappearScale = new Vector3(1, 0, 1);

    public float disappearScaleDuration = 0.5f;

    public void ShowWarning(Action OnComplete)
    {
        SoundManager.instance.PlaySound("Warning_Pop_Up_01");

        warning.SetActive(true);

        warning.transform.localScale = warningAppearStartScale;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(warning.transform.DOScale(warningAppearEndScale, appearScaleDuration));
        sequence.SetDelay(afterAppearWaitTime);
        sequence.Append(warning.transform.DOScale(warningDisappearScale, disappearScaleDuration));

        sequence.OnComplete(
            () =>
            {
                warning.SetActive(false);

                OnComplete?.Invoke();
            });
    }
}
