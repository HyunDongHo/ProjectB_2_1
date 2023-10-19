using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSceneManagers : MonoBehaviour
{
    // 비동기 씬 교체 때 allowSceneActivation 이 False 일때 Progress가 0.9 에서 멈춤.
    protected const float END_LOADING_PROGRESS = 0.9f;

    public virtual void Init(AsyncOperation asyncOperation)
    {
        if (FadeInOut.instance.fadePanel != null)
            FadeInOut.instance.FadeSet(0);

        StartCoroutine(DelayWhileLoading(asyncOperation));
    }

    protected virtual IEnumerator DelayWhileLoading(AsyncOperation asyncOperation)
    {
        yield return null;
    }
}
