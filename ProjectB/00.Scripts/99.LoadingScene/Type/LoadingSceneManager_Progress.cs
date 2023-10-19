using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingSceneManager_Progress : LoadingSceneManagers
{
    public Sprite[] backgroundSprites;

    public Image backgroundImage;
    public Image loadProgress;

    private void Awake()
    {
        backgroundImage.sprite = backgroundSprites[Random.Range(0, backgroundSprites.Length)];

        loadProgress.fillAmount = 0;
    }

    protected override IEnumerator DelayWhileLoading(AsyncOperation asyncOperation)
    {
        yield return StartCoroutine(base.DelayWhileLoading(asyncOperation));

        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            loadProgress.DOFillAmount(asyncOperation.progress / END_LOADING_PROGRESS, Time.deltaTime).OnComplete(() => asyncOperation.allowSceneActivation = true);
            yield return null;
        }
    }
}
