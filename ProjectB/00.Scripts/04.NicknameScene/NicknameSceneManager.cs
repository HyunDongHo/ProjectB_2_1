using Scheduler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class NicknameSceneManager : MonoBehaviour
{
    public Nickname nicknamePopup;
    public CustomizingPanel customizingPanel;
    public PlayerModel playerModel;

    public VideoPlayer videoPlayer;
    public VideoClip[] introVideoClips;
    private int currentIntroIndex = 0;

    public int availableSkipIntroIndex = 0;

    public Button skipButton;

    public string NowHairName = "";
    public string NowHairColorName = "";
    public string NowFaceColorName = "";

    private void Awake()
    {
        AddEvent();
    }

    private void Start()
    {
        FadeInOut.instance.FadeSet(FadeInOut.FADE_IN);
        PlayerAppearPose();
        //nicknamePopup.gameObject.SetActive(true);
        //PlayIntroVideo(currentIntroIndex);
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    private void AddEvent()
    {
        nicknamePopup.nickCreateComp = () => 
        { 
            skipButton.gameObject.SetActive(true);
            nicknamePopup.gameObject.SetActive(false);
            customizingPanel.gameObject.SetActive(false);
            PlayIntroVideo(0); 
        };

        skipButton.onClick.AddListener(EndAllIntroVideo);
    }

    private void RemoveEvent()
    {
        nicknamePopup.nickCreateComp = null;
        skipButton.onClick.RemoveListener(EndAllIntroVideo);
    }

    private void PlayerAppearPose()
    {
        playerModel.animationControl.PlayAnimation("Lobby_Idle_Landing", isRepeat: false,
            OnAnimationEnd: () =>
                playerModel.PlayIdleAnimation(PlayerIdleType.Weapon_On)
            );
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
        SceneSettingManager.instance.LoadNicknameToLobbyStageScene();
    }
}
