using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class IntroSceneManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public VideoClip[] introVideoClips;
    private int currentIntroIndex = 0;

    public int availableSkipIntroIndex = 0;

    //public Button skipButton;

    private void Awake()
    {
        AddEvent();
    }

    private void Start()
    {
        FadeInOut.instance.FadeSet(FadeInOut.FADE_IN);

        PlayIntroVideo(currentIntroIndex);
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    private void AddEvent()
    {
    }

    private void RemoveEvent()
    {
    }

    private void PlayIntroVideo(int videoIndex)
    {
        videoPlayer.clip = introVideoClips[currentIntroIndex];
        videoPlayer.Prepare();

        videoPlayer.prepareCompleted += (prepardData) =>
        {
            //if(currentIntroIndex >= availableSkipIntroIndex) skipButton.gameObject.SetActive(true);

            videoPlayer.Play();
            videoPlayer.loopPointReached +=
                (reachedData) =>
                {
                    if (videoPlayer.time > 0.0f)
                    {
                        if (introVideoClips.Length <= ++currentIntroIndex)
                        {
                            EndAllIntroVideo();
                            return;
                        }

                        PlayIntroVideo(currentIntroIndex);
                    }
                };
        };
    }

    private void EndAllIntroVideo()
    {
        SceneSettingManager.instance.LoadAccountScene(isFade: true);
    }
}
