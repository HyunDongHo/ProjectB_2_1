using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameStageIntroManager : MonoBehaviour
{
    private Action OnEnd;

    public VideoPlayer videoPlayer;
    public VideoClip introVideoClip;

    public Button skipButton;

    private void Awake()
    {
        AddEvent();
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }
    
    private void AddEvent()
    {
        skipButton.onClick.AddListener(EndIntroVideo);
    }

    private void RemoveEvent()
    {
        skipButton.onClick.RemoveListener(EndIntroVideo);
    }

    public void SetIntroVideo(Action OnEnd)
    {
        this.OnEnd = OnEnd;

        PlayIntroVideo();
    }

    private void PlayIntroVideo()
    {
        videoPlayer.clip = introVideoClip;
        videoPlayer.Prepare();

        videoPlayer.prepareCompleted += (prepardData) =>
        {
            FadeInOut.instance.FadeSet(FadeInOut.FADE_IN);

            skipButton.gameObject.SetActive(true);

            videoPlayer.Play();
            videoPlayer.loopPointReached +=
                (reachedData) =>
                {
                    if (videoPlayer.time > 0.0f)
                    {
                        EndIntroVideo();
                        return;
                    }
                };
        };
    }

    private void EndIntroVideo()
    {
        OnEnd?.Invoke();
    }
}
