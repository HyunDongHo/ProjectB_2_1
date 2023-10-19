using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Scheduler;

public class LoadingSceneManager_Video : LoadingSceneManagers
{
    public VideoPlayer videoPlayer;
    public VideoClip videoClip;

    private bool isLoadingEnd = false;

    protected override IEnumerator DelayWhileLoading(AsyncOperation asyncOperation)
    {
        yield return StartCoroutine(base.DelayWhileLoading(asyncOperation));

        PlayVideo(asyncOperation);
    }

    private void PlayVideo(AsyncOperation asyncOperation)
    {
        asyncOperation.allowSceneActivation = false;

        videoPlayer.clip = videoClip;
        videoPlayer.Prepare();

        videoPlayer.prepareCompleted += (prepardData) =>
        {
            FadeInOut.instance.FadeIn(DefineManager.DEFAULT_FADE_DURATION);

            videoPlayer.Play();
            videoPlayer.loopPointReached +=
                (reachedData) =>
                {
                    if (videoPlayer.time > 0.0f)
                    {
                        Timer.instance.TimerStart(new TimerBuffer((float)videoPlayer.length),
                            OnFrame: () =>
                            {
                                if (asyncOperation.progress >= END_LOADING_PROGRESS && !isLoadingEnd)
                                {
                                    isLoadingEnd = true;
                                    FadeInOut.instance.FadeOut(DefineManager.DEFAULT_FADE_DURATION, () => asyncOperation.allowSceneActivation = true);
                                }
                            });
                        return;
                    }
                };
        };
    }
}
